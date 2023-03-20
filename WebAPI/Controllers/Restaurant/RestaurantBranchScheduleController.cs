using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantBranchScheduleController : ControllerBase
    {
        private readonly IRestaurantBranchScheduleService _service;
        private readonly IMapper _mapper;

        public RestaurantBranchScheduleController(IRestaurantBranchScheduleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }


        [HttpGet("GetAll/RestaurantBranches/{branchId}")]
        public async Task<IActionResult> GetAll(long branchId)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantBranchScheduleDTO>>(await _service.GetRestaurantBranchScheduleByBranch(branchId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<RestaurantBranchScheduleDTO> List = _mapper.Map<IEnumerable<RestaurantBranchScheduleDTO>>(await _service.GetRestaurantBranchScheduleById(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Add(RestaurantBranchScheduleDTO Model)
        {           
            IEnumerable<RestaurantBranchScheduleDTO> isExist = _mapper.Map<IEnumerable<RestaurantBranchScheduleDTO>>(await _service.GetScheduleByDay(Model.Day, Model.OpeningTime , Model.ClosingTime , Model.RestaurantBranchId));
            if (!isExist.Any())
            {
                return Ok(_mapper.Map<RestaurantBranchScheduleDTO>(await _service.AddRestaurantBranchScheduleAsync(_mapper.Map<RestaurantBranchSchedule>(Model))));
            }

            return Conflict();
        }

        [HttpPut]
        public async Task<IActionResult> Update(RestaurantBranchScheduleDTO Model)
        {
          
            IEnumerable<RestaurantBranchSchedule> isBranchExist = await _service.GetScheduleByDay(Model.Day, Model.OpeningTime , Model.ClosingTime , Model.RestaurantBranchId, Model.Id);
            if (!isBranchExist.Any())
            {
                return Ok(_mapper.Map<RestaurantBranchScheduleDTO>(await _service.UpdateRestaurantBranchScheduleAsync(_mapper.Map<RestaurantBranchSchedule>(Model))));
            }

            return Conflict();
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            await _service.ArchiveRestaurantBranchScheduleAsync(Id);
            return Ok();
        }
    }
}
