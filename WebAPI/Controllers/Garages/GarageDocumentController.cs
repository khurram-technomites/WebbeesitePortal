using AutoMapper;
using HelperClasses.DTOs.Garage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;

namespace WebAPI.Controllers.Garages
{
    [Route("api/Garage")]
    [ApiController]
    [Authorize]
    public class GarageDocumentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGarageDocumentService _garageDocumentService;
        private readonly IFTPUpload _ftpUpload;
        public GarageDocumentController(IMapper mapper, IGarageDocumentService garageDocumentService, IFTPUpload ftpUpload)
        {
            _mapper = mapper;
            _garageDocumentService = garageDocumentService;
            _ftpUpload = ftpUpload;
        }

        [HttpGet("{GarageId}/Document")]
        public async Task<IActionResult> GetAll(long GarageId)
        {
            return Ok(_mapper.Map<IEnumerable<GarageDocumentDTO>>(await _garageDocumentService.GetByGarage(GarageId)));
        }

        [HttpGet("Document/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<Models.GarageDocument> result = await _garageDocumentService.GetByID(Id);
            return Ok(_mapper.Map<GarageDocumentDTO>(result.FirstOrDefault()));
        }

        [HttpPost("Document")]
        public async Task<IActionResult> AddDocument(GarageDocumentDTO Model)
        {
            if (Model.Path is not null && Model.Path.Contains("Draft"))
            {
                string LogoPath = "/Documents/Garage/" + Model.GarageId + "/";
                if (_ftpUpload.MoveFile(Model.Path, ref LogoPath))
                {
                    Model.Path = LogoPath;
                }
            }

            return Ok(_mapper.Map<GarageDocumentDTO>(await _garageDocumentService.AddDocument(_mapper.Map<Models.GarageDocument>(Model))));
        }

        [HttpDelete("Document/{Id}")]
        public async Task<IActionResult> DeleteDocument(long Id)
        {
            var list = await _garageDocumentService.GetByID(Id);

            if (list.FirstOrDefault().Path.Contains("cdn.fougito.com"))
                _ftpUpload.DeleteFile(Uri.UnescapeDataString(list.FirstOrDefault().Path));

            await _garageDocumentService.DeleteRecord(Id);

            return Ok();
        }
    }
}
