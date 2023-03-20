using AutoMapper;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Supplier
{
    [Route("api/Supplier")]
    [ApiController]
    //[Authorize]
    public class SupplierDocumentController : ControllerBase
    {

        private readonly ISupplierDocumentService _supplierDocumentService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;
        public SupplierDocumentController(ISupplierDocumentService supplierDocumentService, IMapper mapper, IFTPUpload fTPUpload)
        {
            _supplierDocumentService = supplierDocumentService;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("{SupplierId}/Document")]
        public async Task<IActionResult> GetBySupplierId(long SupplierId)
        {
            return Ok(_mapper.Map<IEnumerable<SupplierDocumentDTO>>(await _supplierDocumentService.GetAllBySupplierId(SupplierId)));
        }

        [HttpPost("Document")]
        public async Task<IActionResult> AddDocument(SupplierDocumentDTO Model)
        {
            string LogoPath = "/Document/Supplier/";
            if (_fTPUpload.MoveFile(Model.Path, ref LogoPath))
            {
                Model.Path = LogoPath;
            }
            return Ok(_mapper.Map<SupplierDocumentDTO>(await _supplierDocumentService.AddSupplierDocumentAsync(_mapper.Map<SupplierDocument>(Model))));
        }

        [HttpPut("Document")]
        public async Task<IActionResult> UpdateDocument(SupplierDocumentDTO Model)
        {
            return Ok(_mapper.Map<SupplierDocumentDTO>(await _supplierDocumentService.UpdateSupplierDocumentAsync(_mapper.Map<SupplierDocument>(Model))));
        }

        [HttpDelete("Document/{Id}")]
        public async Task<IActionResult> DeleteDocument(long Id)
        {
            IEnumerable<SupplierDocument> list = await _supplierDocumentService.GetByIdAsync(Id);

            _fTPUpload.DeleteFile(list.FirstOrDefault().Path.Replace(" ", "%20"));

            await _supplierDocumentService.DeleteSupplierDocumentAsync(Id);
            return Ok();
        }


    }
}
