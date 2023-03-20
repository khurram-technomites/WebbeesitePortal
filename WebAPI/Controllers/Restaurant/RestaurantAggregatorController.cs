using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/Restaurant")]
    [ApiController]
    public class RestaurantAggregatorController : ControllerBase
    {
        private readonly IRestaurantAggregatorService _restaurantAggregatorService;
        private readonly IMapper _mapper;

        public RestaurantAggregatorController(IRestaurantAggregatorService restaurantAggregatorService, IMapper mapper)
        {
            _restaurantAggregatorService = restaurantAggregatorService;
            _mapper = mapper;
        }

        [HttpGet("Aggregator")]
        public async Task<IActionResult> GetAll()
        {
            var Aggregator = _mapper.Map<IEnumerable<RestaurantAggregatorDTO>>(await _restaurantAggregatorService.GetAllAsync());
            return Ok(Aggregator);
        }

        [HttpGet("Aggregator/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            var Aggregator = _mapper.Map<IEnumerable<RestaurantAggregatorDTO>>(await _restaurantAggregatorService.GetByIdAsync(Id));
            return Ok(Aggregator.FirstOrDefault());
        }

        [HttpGet("{Id}/Aggregator")]
        public async Task<IActionResult> GetByRestaurantIdAsync(long Id)
        {
            var Aggregator = _mapper.Map<IEnumerable<RestaurantAggregatorDTO>>(await _restaurantAggregatorService.GetByRestaurantIdAsync(Id));
            return Ok(Aggregator);
        }

        [HttpGet("Aggregator/ByBranch/{Id}")]
        public async Task<IActionResult> GetByBranchIdAsync(long Id)
        {
            var Aggregator = _mapper.Map<IEnumerable<RestaurantAggregatorDTO>>(await _restaurantAggregatorService.GetByRestaurantBranchIdAsync(Id));
            return Ok(Aggregator);
        }

        [HttpPost("Aggregator")]
        public async Task<IActionResult> AddAggregator(RestaurantAggregatorDTO Model)
        {
            return Ok(_mapper.Map<RestaurantAggregatorDTO>(await _restaurantAggregatorService.AddRestaurantAggregatorAsync(_mapper.Map<RestaurantAggregator>(Model))));
        }
        [HttpPut("Aggregator")]
        public async Task<IActionResult> UpdateAggregator(RestaurantAggregatorDTO Model)
        {
            IEnumerable<RestaurantAggregator> list = await _restaurantAggregatorService.GetByIdAsync(Model.Id);
            RestaurantAggregator restaurantAggregator = list.FirstOrDefault();
            restaurantAggregator = _mapper.Map(Model, restaurantAggregator);

            return Ok(_mapper.Map<RestaurantAggregatorDTO>(await _restaurantAggregatorService.UpdateRestaurantAggregatorAsync(_mapper.Map<RestaurantAggregator>(restaurantAggregator))));
        }

        [HttpDelete("Aggregator")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantAggregatorDTO>>(await _restaurantAggregatorService.ArchiveRestaurantAggregatorAsync(Id)));
        }
    }
}
