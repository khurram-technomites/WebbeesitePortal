using AutoMapper;
using HelperClasses.DTOs.Restaurant;
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

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/Restaurant")]
    [ApiController]
    [Authorize(Roles = "Admin, Restaurant Manager")]
    public class RestaurantDocumentController : ControllerBase
    {
        private readonly IRestaurantDocumentService _service;
        private readonly IFTPUpload _ftpUpload;
        private readonly IMapper _mapper;

        public RestaurantDocumentController(IRestaurantDocumentService service, IMapper mapper, IFTPUpload ftpUpload)
        {
            _service = service;
            _ftpUpload = ftpUpload;
            _mapper = mapper;
        }

        [HttpGet("{RestaurantId}/Document")]
        public async Task<IActionResult> GetByRestaurant(long RestaurantId)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantDocumentDTO>>(await _service.GetAllByRestaurantAsync(RestaurantId)));
        }

        [HttpGet("Document/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<RestaurantDocumentDTO> List = _mapper.Map<IEnumerable<RestaurantDocumentDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost("Document")]
        public async Task<IActionResult> AddDocument(RestaurantDocumentDTO Model)
        {
            return Ok(_mapper.Map<RestaurantDocumentDTO>(await _service.AddRestaurantDocumentAsync(_mapper.Map<RestaurantDocument>(Model))));
        }

        [HttpPut("Document")]
        public async Task<IActionResult> UpdateDocument(RestaurantDocumentDTO Model)
        {
            return Ok(_mapper.Map<RestaurantDocumentDTO>(await _service.AddRestaurantDocumentAsync(_mapper.Map<RestaurantDocument>(Model))));
        }

        [HttpDelete("Document/{Id}")]
        public async Task<IActionResult> DeleteDocument(long Id)
        {
            var list = await _service.GetByIdAsync(Id);
            if (list.FirstOrDefault().Path.Contains("cdn.fougito.com"))
                _ftpUpload.DeleteFile(Uri.UnescapeDataString(list.FirstOrDefault().Path));

            await _service.DeleteRestaurantDocumentAsync(Id);

            return Ok();
        }
    }
}
