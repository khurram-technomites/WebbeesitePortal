using AutoMapper;
using HelperClasses.DTOs.Garage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;

namespace WebAPI.Controllers.Garages
{
    [Route("api/Garage")]
    [ApiController]
    public class GarageImageController : ControllerBase
    {
        private readonly IGarageImageService _garageImageService;
        private readonly IMapper _mapper;

        public GarageImageController(IGarageImageService garageImageService, IMapper mapper)
        {
            _garageImageService = garageImageService;
            _mapper = mapper;
        }

        [HttpGet("{GarageId}/Image")]
        public async Task<IActionResult> GetByGarage(long GarageId)
        {
            return Ok(_mapper.Map<IEnumerable<GarageImageDTO>>(await _garageImageService.GetImagesByGarage(GarageId)));
        }

        [HttpDelete("Image/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            await _garageImageService.DeleteGarageImage(Id);
            return Ok();
        }
    }
}
