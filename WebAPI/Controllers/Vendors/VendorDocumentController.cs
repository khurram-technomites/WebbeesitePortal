using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;

namespace WebAPI.Controllers.Vendors
{
    [Route("api/Vendor")]
    [ApiController]
    [Authorize]
    public class VendorDocumentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVendorDocumentService _vendorDocumentService;
        private readonly IFTPUpload _ftpUpload;
        public VendorDocumentController(IMapper mapper, IVendorDocumentService vendorDocumentService, IFTPUpload ftpUpload)
        {
            _mapper = mapper;
            _vendorDocumentService = vendorDocumentService;
            _ftpUpload = ftpUpload;
        }

        [HttpGet("{VendorId}/Document")]
        public async Task<IActionResult> GetAll(long VendorId)
        {
            return Ok(_mapper.Map<IEnumerable<VendorDocumentDTO>>(await _vendorDocumentService.GetByVendor(VendorId)));
        }

        [HttpGet("Document/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<Models.VendorDocument> result = await _vendorDocumentService.GetByID(Id);
            return Ok(_mapper.Map<VendorDocumentDTO>(result.FirstOrDefault()));
        }

        [HttpPost("Document")]
        public async Task<IActionResult> AddDocument(VendorDocumentDTO Model)
        {
            if (Model.Path is not null && Model.Path.Contains("Draft"))
            {
                string LogoPath = "/Documents/Vendor/" + Model.VendorId + "/";
                if (_ftpUpload.MoveFile(Model.Path, ref LogoPath))
                {
                    Model.Path = LogoPath;
                }
            }

            return Ok(_mapper.Map<VendorDocumentDTO>(await _vendorDocumentService.AddDocument(_mapper.Map<Models.VendorDocument>(Model))));
        }

        [HttpDelete("Document/{Id}")]
        public async Task<IActionResult> DeleteDocument(long Id)
        {
            var list = await _vendorDocumentService.GetByID(Id);

            if (list.FirstOrDefault().Path.Contains("cdn.webbeesite.com"))
                _ftpUpload.DeleteFile(Uri.UnescapeDataString(list.FirstOrDefault().Path));

            await _vendorDocumentService.DeleteRecord(Id);

            return Ok();
        }
    }
}
