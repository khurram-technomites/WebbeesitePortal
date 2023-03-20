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
    public class RestaurantCashDenominationController : ControllerBase
    {
        private readonly IRestaurantCashDenominationService _restaurantCashDenominationService;
        private readonly IMapper _mapper;

        public RestaurantCashDenominationController(IRestaurantCashDenominationService restaurantCashDenominationService, IMapper mapper)
        {
            _restaurantCashDenominationService = restaurantCashDenominationService;
            _mapper = mapper;
        }

        [HttpGet("CashDenomination")]
        public async Task<IActionResult> GetAllAsync()
        {
            var CashDenomination= _mapper.Map<IEnumerable<RestaurantCashDenominationDTO>>(await _restaurantCashDenominationService.GetAllAsync());
            return Ok(CashDenomination);
        }

        [HttpGet("CashDenomination/{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            var CashDenomination = _mapper.Map<IEnumerable<RestaurantCashDenominationDTO>>(await _restaurantCashDenominationService.GetByIdAsync(Id));
            return Ok(CashDenomination.FirstOrDefault());

        }

        [HttpGet("CashDenomination/ByBalanceSheet/{Id}")]
        public async Task<IActionResult> GetByBalanceSheetId(long Id)
        {
            var CashDenomination = _mapper.Map<IEnumerable<RestaurantCashDenominationDTO>>(await _restaurantCashDenominationService.GetByBalanceSheetId(Id));
            return Ok(CashDenomination);

        }

        [HttpPost("CashDenomination")]
        public async Task<IActionResult> AddCashDenomination(RestaurantCashDenominationDTO Model)
        {
            return Ok(_mapper.Map<RestaurantCashDenominationDTO>(await _restaurantCashDenominationService.AddCashDenominationAsync(_mapper.Map<RestaurantCashDenomination>(Model))));
        }

        [HttpPut("CashDenomination")]
        public async Task<IActionResult> UpdateCashDenomination(RestaurantCashDenominationDTO Model)
        {
            IEnumerable< RestaurantCashDenomination> list = await _restaurantCashDenominationService.GetByIdAsync(Model.Id);
            RestaurantCashDenomination restaurantCashDenomination = list.FirstOrDefault();
            restaurantCashDenomination = _mapper.Map(Model, restaurantCashDenomination);
            return Ok(_mapper.Map<RestaurantCashDenominationDTO>(await _restaurantCashDenominationService.UpdateCashDenominationAsync(_mapper.Map<RestaurantCashDenomination>(restaurantCashDenomination))));
        }

        [HttpDelete("CashDenomination")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantCashDenominationDTO>>(await _restaurantCashDenominationService.ArchiveCashDenominationAsync(Id)));
        }
    }
}
