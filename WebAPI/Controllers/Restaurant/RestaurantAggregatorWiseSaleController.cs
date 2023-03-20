using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.Services.Domains;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/Restaurant")]
    [ApiController]
    public class RestaurantAggregatorWiseSaleController : ControllerBase
    {
        private readonly IRestaurantAggregatorWiseSaleService _restaurantAggregatorWiseSaleService;
        private readonly IMapper _mapper;

        public RestaurantAggregatorWiseSaleController(IRestaurantAggregatorWiseSaleService restaurantAggregatorWiseSaleService, IMapper mapper)
        {
            _restaurantAggregatorWiseSaleService = restaurantAggregatorWiseSaleService;
            _mapper = mapper;
        }

        [HttpGet("AggregatorWiseSale")]
        public async Task<IActionResult> GetAllAsync()
        {
            var AggregatorWiseSale = _mapper.Map<IEnumerable<RestaurantAggregatorWiseSaleDTO>>(await _restaurantAggregatorWiseSaleService.GetAllAsync());
            return Ok(AggregatorWiseSale);
        }

        [HttpGet("AggregatorWiseSale/{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            var AggregatorWiseSale = _mapper.Map<IEnumerable<RestaurantAggregatorWiseSaleDTO>>(await _restaurantAggregatorWiseSaleService.GetByIdAsync(Id));
            return Ok(AggregatorWiseSale.FirstOrDefault());
        }

        [HttpGet("AggregatorWiseSale/ByOrder/{Id}")]
        public async Task<IActionResult> GetByOrderId(long Id)
        {
            var AggregatorWiseSale = _mapper.Map<IEnumerable<RestaurantAggregatorWiseSaleDTO>>(await _restaurantAggregatorWiseSaleService.GetOrderIdAsync(Id));
            return Ok(AggregatorWiseSale);
        }

        [HttpGet("AggregatorWiseSale/ByBalanceSheet/{Id}")]
        public async Task<IActionResult> GetBalanceSheetId(long Id)
        {
            var AggregatorWiseSale = _mapper.Map<IEnumerable<RestaurantAggregatorWiseSaleDTO>>(await _restaurantAggregatorWiseSaleService.GetBalanceSheetIdAsync(Id));
            return Ok(AggregatorWiseSale);
        }

        [HttpGet("AggregatorWiseSale/Aggregator/{Id}")]
        public async Task<IActionResult> GetAggregatorId(long Id)
        {
            var AggregatorWiseSale = _mapper.Map<IEnumerable<RestaurantAggregatorWiseSaleDTO>>(await _restaurantAggregatorWiseSaleService.GetRestaurantAggregatorIdAsync(Id));
            return Ok(AggregatorWiseSale);
        }

        [HttpPost("AggregatorWiseSale")]
        public async Task<IActionResult> AddAggregatorWiseSale(RestaurantAggregatorWiseSaleDTO Model)
        {
            return Ok(_mapper.Map<RestaurantAggregatorWiseSaleDTO>(await _restaurantAggregatorWiseSaleService.AddAggregatorWiseSale(_mapper.Map<RestaurantAggregatorWiseSale>(Model))));
        }

        [HttpPut("AggregatorWiseSale")]
        public async Task<IActionResult> UpdateAggregatorWiseSale(RestaurantAggregatorWiseSaleDTO Model)
        {
            IEnumerable<RestaurantAggregatorWiseSale> list = await _restaurantAggregatorWiseSaleService.GetByIdAsync(Model.Id);
            RestaurantAggregatorWiseSale restaurantAggregatorWiseSale = list.FirstOrDefault();
            restaurantAggregatorWiseSale = _mapper.Map(Model, restaurantAggregatorWiseSale);

            return Ok(_mapper.Map<RestaurantAggregatorWiseSaleDTO>(await _restaurantAggregatorWiseSaleService.UpdateAggregatorWiseSale(_mapper.Map<RestaurantAggregatorWiseSale>(restaurantAggregatorWiseSale))));
        }

        [HttpDelete("AggregatorWiseSale/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantAggregatorWiseSaleDTO>>(await _restaurantAggregatorWiseSaleService.ArchiveAggregatorWiseSale(Id)));
        }


    }
}
