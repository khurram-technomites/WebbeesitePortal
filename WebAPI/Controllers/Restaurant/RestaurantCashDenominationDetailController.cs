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
    public class RestaurantCashDenominationDetailController : ControllerBase
    {
        private readonly IRestaurantCashDenominationDetailService _restaurantCashDenominationDetailService;
        private readonly IMapper _mapper;
        public RestaurantCashDenominationDetailController(IRestaurantCashDenominationDetailService restaurantCashDenominationDetailService, IMapper mapper)
        {
            _restaurantCashDenominationDetailService = restaurantCashDenominationDetailService;
            _mapper = mapper;
        }

        [HttpGet("CashDenominationDetail")]
        public async Task<IActionResult> GetAllAsync()
        {
            var CashDenominationDetail = _mapper.Map<IEnumerable<RestaurantCashDenominationDetailDTO>>(await _restaurantCashDenominationDetailService.GetAllAsync());
            return Ok(CashDenominationDetail);
        }

        [HttpGet("CashDenominationDetail/{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            var CashDenominationDetail = _mapper.Map<IEnumerable<RestaurantCashDenominationDetailDTO>>(await _restaurantCashDenominationDetailService.GetByIdAsync(Id));
            return Ok(CashDenominationDetail.FirstOrDefault());
        }

        [HttpGet("CashDenominationDetail/ByCurrencyNote/{Id}")]
        public async Task<IActionResult> GetByCurrencyNoteId(long Id)
        {
            var CashDenomination = _mapper.Map<IEnumerable<RestaurantCashDenominationDetailDTO>>(await _restaurantCashDenominationDetailService.GetByCurrencyNoteIdAsync(Id));
            return Ok(CashDenomination);
        }

        [HttpGet("CashDenominationDetail/ByCashDenomination/{Id}")]
        public async Task<IActionResult> GetByCashDenominationId(long Id)
        {
            var CashDenomination = _mapper.Map<IEnumerable<RestaurantCashDenominationDetailDTO>>(await _restaurantCashDenominationDetailService.GetByCashDenominationIdAsync(Id));
            return Ok(CashDenomination);
        }

        [HttpPost("CashDenominationDetail")]
        public async Task<IActionResult> AddCashDenomination(RestaurantCashDenominationDetailDTO Model)
        {
            return Ok(_mapper.Map<RestaurantCashDenominationDetailDTO>(await _restaurantCashDenominationDetailService.AddCashDenominationDetail(_mapper.Map<RestaurantCashDenominationDetail>(Model))));
        }

        [HttpPut("CashDenominationDetail")]
        public async Task<IActionResult> UpdateCashDenomination(RestaurantCashDenominationDetailDTO Model)
        {
            IEnumerable<RestaurantCashDenominationDetail> list = await _restaurantCashDenominationDetailService.GetByIdAsync(Model.Id);
            RestaurantCashDenominationDetail restaurantCashDenomination = list.FirstOrDefault();
            restaurantCashDenomination = _mapper.Map(Model, restaurantCashDenomination);
            return Ok(_mapper.Map<RestaurantCashDenominationDetailDTO>(await _restaurantCashDenominationDetailService.UpdateCashDenominationDetail(_mapper.Map<RestaurantCashDenominationDetail>(restaurantCashDenomination))));
        }

        [HttpDelete("CashDenominationDetail/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantCashDenominationDetailDTO>>(await _restaurantCashDenominationDetailService.ArchiveCashDenominationDetail(Id)));
        }
    }
}
