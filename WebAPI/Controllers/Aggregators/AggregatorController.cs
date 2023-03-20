using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Aggregators;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantCashierStaff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;
using WebAPI.Services.Domains;

namespace WebAPI.Controllers.Aggregators
{
    [Route("api/Aggregator")]
    [ApiController]
    [Authorize(Roles = "Admin , RestaurantOwner , RestaurantCashierStaff")]
    public class AggregatorController : ControllerBase
    {
        private readonly IAggregatorService _aggregatorService;
        private readonly IMapper _mapper;

        public AggregatorController(IAggregatorService aggregatorService, IMapper mapper)
        {
            _aggregatorService = aggregatorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllasync()
        {
            return Ok(new SuccessResponse<IEnumerable<AggregatorDTO>>("Data received successfully", _mapper.Map<IEnumerable<AggregatorDTO>>(await _aggregatorService.GetAllAsync())));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {

            return Ok(new SuccessResponse<IEnumerable<AggregatorDTO>>("Data received successfully", _mapper.Map<IEnumerable<AggregatorDTO>>(await _aggregatorService.GetByIdAsync(Id))));
        }

        [HttpGet("ByRestaurant/{Id}")]
        public async Task<IActionResult> GetByRestaurantIdAsync(long Id)
        {
            return Ok(new SuccessResponse<IEnumerable<AggregatorDTO>>("Data received successfully", _mapper.Map<IEnumerable<AggregatorDTO>>(await _aggregatorService.GetByRestaurantIdAsync(Id))));
        }

        [HttpGet("ByRestaurantBranch/{Id}")]
        public async Task<IActionResult> GetByRestaurantBranchIdAsync(long Id)
        {
            return Ok(new SuccessResponse<IEnumerable<AggregatorDTO>>("Data received successfully", _mapper.Map<IEnumerable<AggregatorDTO>>(await _aggregatorService.GetByRestaurantBranchIdAsync(Id))));
        }

        [Authorize(Roles = "Admin,RestaurantOwner")]
        [HttpPost]
        public async Task<IActionResult> AddAggregator(AggregatorDTO Model)
        {
            return Ok(new SuccessResponse<AggregatorDTO>("Data added successfully", _mapper.Map<AggregatorDTO>(await _aggregatorService.AddAggregator(_mapper.Map<Aggregator>(Model)))));
        }

        [Authorize(Roles = "Admin,RestaurantOwner ")]
        [HttpPut]
        public async Task<IActionResult> UpdateAggregator(AggregatorDTO Model)
        {
            IEnumerable<Aggregator> list = await _aggregatorService.GetByIdAsync(Model.Id);
            Aggregator aggregator = list.FirstOrDefault();
            aggregator = _mapper.Map(Model, aggregator);

            Model = _mapper.Map<AggregatorDTO>(await _aggregatorService.UpdateAggregator(aggregator));

            return Ok(Model);
        }

        [Authorize(Roles = "Admin,RestaurantOwner ")]
        [HttpGet("{Id}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<Aggregator> aggregator = await _aggregatorService.GetByIdAsync(Id);
            Aggregator staff = aggregator.FirstOrDefault();

            if (staff.Status == Enum.GetName(typeof(Status), Status.Active))
                staff.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                staff.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _aggregatorService.UpdateAggregator(staff));
        }

        [Authorize(Roles = "Admin,RestaurantOwner ")]
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(new SuccessResponse<AggregatorDTO>("Data deleted successfully", _mapper.Map<AggregatorDTO>(await _aggregatorService.ArchiveAggregator(Id))));
        }
    }
}
