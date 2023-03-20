using AutoMapper;
using HelperClasses.DTOs.Garage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Garages
{
    [Route("api/Garage")]
    [ApiController]
    [Authorize]
    public class GarageScheduleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGarageScheduleService _garageScheduleService;
        public GarageScheduleController(IMapper mapper, IGarageScheduleService garageScheduleService)
        {
            _mapper = mapper;
            _garageScheduleService = garageScheduleService;
        }

        [HttpGet("{GarageId}/Schedule")]
        public async Task<IActionResult> GetAll(long GarageId)
        {
            return Ok(_mapper.Map<IEnumerable<GarageScheduleDTO>>(await _garageScheduleService.GetByGarage(GarageId)));
        }

        [HttpPut("Schedule")]
        public async Task<IActionResult> AddandUpdate(IEnumerable<GarageScheduleDTO> List)
        {
            return Ok(await _garageScheduleService.AddAndUpdateRange(_mapper.Map<IEnumerable<GarageSchedule>>(List)));
        }

        [HttpDelete("Schedule/{Id}")]
        public async Task<IActionResult> DeleteSchedule(long Id)
        {
            await _garageScheduleService.DeleteSchedule(Id);
            return Ok();
        }
    }
}
