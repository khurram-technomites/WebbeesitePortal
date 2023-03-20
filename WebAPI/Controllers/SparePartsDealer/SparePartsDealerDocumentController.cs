using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.SparePartsDealer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.SparePartsDealer
{
    [Route("api/SparePartsDealerDocument")]
    [ApiController]
    [Authorize(Roles = "Admin , SparePartDealer")]
    public class SparePartsDealerDocumentController : ControllerBase
    {
        private readonly ISparePartsDealerDocumentService _service;
        private readonly IFTPUpload _ftpUpload;
        private readonly IMapper _mapper;

        public SparePartsDealerDocumentController(ISparePartsDealerDocumentService service, IMapper mapper, IFTPUpload ftpUpload)
        {
            _service = service;
            _ftpUpload = ftpUpload;
            _mapper = mapper;
        }
        [HttpGet("{Id}/Document")]
        public async Task<IActionResult> GetByRestaurant(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartsDealerDocumentDTO>>(await _service.GetAllBySparePartsAsync(Id)));
        }

        [HttpGet("Document/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<SparePartsDealerDocumentDTO> List = _mapper.Map<IEnumerable<SparePartsDealerDocumentDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost("Document")]
        public async Task<IActionResult> AddDocument(SparePartsDealerDocumentDTO Model)
        {
            return Ok(_mapper.Map<SparePartsDealerDocumentDTO>(await _service.AddSparePartsDocumentAsync(_mapper.Map<SparePartsDealerDocument>(Model))));
        }

        [HttpPut("Document")]
        public async Task<IActionResult> UpdateDocument(SparePartsDealerDocumentDTO Model)
        {
            return Ok(_mapper.Map<SparePartsDealerDocumentDTO>(await _service.UpdateSparePartsDocumentAsync(_mapper.Map<SparePartsDealerDocument>(Model))));
        }

        [HttpDelete("Document/{Id}")]
        public async Task<IActionResult> DeleteDocument(long Id)
        {
            var list = await _service.GetByIdAsync(Id);
            if (list.FirstOrDefault().Path.Contains("cdn.fougito.com"))
                _ftpUpload.DeleteFile(Uri.UnescapeDataString(list.FirstOrDefault().Path));

            await _service.DeleteSparePartsDocumentAsync(Id);

            return Ok();
        }

    }
}
