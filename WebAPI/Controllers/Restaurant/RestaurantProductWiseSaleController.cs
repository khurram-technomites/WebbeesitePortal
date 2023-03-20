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
    public class RestaurantProductWiseSaleController : ControllerBase
    {
        private readonly IRestaurantProductWiseSaleService _restaurantProductWiseSaleService;
        private readonly IMapper _mapper;

        public RestaurantProductWiseSaleController(IRestaurantProductWiseSaleService restaurantProductWiseSaleService, IMapper mapper)
        {
            _restaurantProductWiseSaleService = restaurantProductWiseSaleService;
            _mapper = mapper;
        }

        [HttpGet("ProductWiseSale")]
        public async Task<IActionResult> GetAllasync()
        {
            var ProductWiseSale = _mapper.Map<IEnumerable<RestaurantProductWiseSaleDTO>>(await _restaurantProductWiseSaleService.GetAllAsync());
            return Ok(ProductWiseSale);
        }

        [HttpGet("ProductWiseSale/{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            var ProductWiseSale = _mapper.Map<IEnumerable<RestaurantProductWiseSaleDTO>>(await _restaurantProductWiseSaleService.GetByIdAsync(Id));
            return Ok(ProductWiseSale.FirstOrDefault());
        }

        [HttpGet("ProductWiseSale/BalanceSheet/{Id}")]
        public async Task<IActionResult> GetByBalanceSheetId(long Id)
        {
            var ProductWiseSale = _mapper.Map<IEnumerable<RestaurantProductWiseSaleDTO>>(await _restaurantProductWiseSaleService.GetBalanceSheetIdAsync(Id));
            return Ok(ProductWiseSale);
        }

        [HttpGet("ProductWiseSale/OrderDetail/{Id}")]
        public async Task<IActionResult> GetByOrderDetailId(long Id)
        {
            var ProductWiseSale = _mapper.Map<IEnumerable<RestaurantProductWiseSaleDTO>>(await _restaurantProductWiseSaleService.GetOrderDetailIdAsync(Id));
            return Ok(ProductWiseSale);
        }

        [HttpGet("ProductWiseSale/MenuItem/{Id}")]
        public async Task<IActionResult> GetByMenuItemId(long Id)
        {
            var ProductWiseSale = _mapper.Map<IEnumerable<RestaurantProductWiseSaleDTO>>(await _restaurantProductWiseSaleService.GetMenuItemIdAsync(Id));
            return Ok(ProductWiseSale);
        }

        [HttpPost("ProductWiseSale")]
        public async Task<IActionResult> AddProductWiseSale(RestaurantProductWiseSaleDTO Model)
        {
            return Ok(_mapper.Map<RestaurantProductWiseSaleDTO>(await _restaurantProductWiseSaleService.AddProductWiseSale(_mapper.Map<RestaurantProductWiseSale>(Model))));
        }

        [HttpPut("ProductWiseSale")]
        public async Task<IActionResult> UpdateProductWiseSale(RestaurantProductWiseSaleDTO Model)
        {
            IEnumerable<RestaurantProductWiseSale> list = await _restaurantProductWiseSaleService.GetByIdAsync(Model.Id);
            RestaurantProductWiseSale restaurantProductWiseSale = list.FirstOrDefault();
            restaurantProductWiseSale = _mapper.Map(Model, restaurantProductWiseSale);

            return Ok(_mapper.Map<RestaurantProductWiseSaleDTO>(await _restaurantProductWiseSaleService.UpdateProductWiseSale(_mapper.Map<RestaurantProductWiseSale>(restaurantProductWiseSale))));
        }

        [HttpDelete("ProductWiseSale/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantProductWiseSaleDTO>>(await _restaurantProductWiseSaleService.ArchiveProductWiseSale(Id)));
        }
    }
}
