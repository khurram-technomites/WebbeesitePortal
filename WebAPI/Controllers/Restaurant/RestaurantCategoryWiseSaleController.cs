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
    public class RestaurantCategoryWiseSaleController : ControllerBase
    {
        private readonly IRestaurantCategoryWiseSaleService _restaurantCategoryWiseSaleService;
        private readonly IMapper _mapper;

        public RestaurantCategoryWiseSaleController(IRestaurantCategoryWiseSaleService restaurantCategoryWiseSaleService, IMapper mapper)
        {
            _restaurantCategoryWiseSaleService = restaurantCategoryWiseSaleService;
            _mapper = mapper;
        }

        [HttpGet("CategoryWiseSale")]
        public async Task<IActionResult> GetAllasync()
        {
            var CategoryWiseSale = _mapper.Map<IEnumerable<RestaurantCategoryWiseSaleDTO>>(await _restaurantCategoryWiseSaleService.GetAllAsync());
            return Ok(CategoryWiseSale);
        }

        [HttpGet("CategoryWiseSale/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            var CategoryWiseSale = _mapper.Map<IEnumerable<RestaurantCategoryWiseSaleDTO>>(await _restaurantCategoryWiseSaleService.GetByIdAsync(Id));
            return Ok(CategoryWiseSale);
        }

        [HttpGet("CategoryWiseSale/BalanceSheet/{Id}")]
        public async Task<IActionResult> GetByBalanceSheetId(long Id)
        {
            var CategoryWiseSale = _mapper.Map<IEnumerable<RestaurantCategoryWiseSaleDTO>>(await _restaurantCategoryWiseSaleService.GetBalanceSheetIdAsync(Id));
            return Ok(CategoryWiseSale);
        }

        [HttpGet("CategoryWiseSale/OrderDetail/{Id}")]
        public async Task<IActionResult> GetByOrderDetailId(long Id)
        {
            var CategoryWiseSale = _mapper.Map<IEnumerable<RestaurantCategoryWiseSaleDTO>>(await _restaurantCategoryWiseSaleService.GetOrderDetailIdAsync(Id));
            return Ok(CategoryWiseSale);
        }

        [HttpGet("CategoryWiseSale/ByCategory/{Id}")]
        public async Task<IActionResult> GetCategoryId(long Id)
        {
            var CategoryWiseSale = _mapper.Map<IEnumerable<RestaurantCategoryWiseSaleDTO>>(await _restaurantCategoryWiseSaleService.GetCategoryIdAsync(Id));
            return Ok(CategoryWiseSale);
        }

        [HttpPost("CategoryWiseSale")]
        public async Task<IActionResult> AddProductWiseSale(RestaurantCategoryWiseSaleDTO Model)
        {
            return Ok(_mapper.Map<RestaurantCategoryWiseSaleDTO>(await _restaurantCategoryWiseSaleService.AddCategoryWiseSale(_mapper.Map<RestaurantCategoryWiseSale>(Model))));
        }

        [HttpPut("CategoryWiseSale")]
        public async Task<IActionResult> UpdateProductWiseSale(RestaurantCategoryWiseSaleDTO Model)
        {
            IEnumerable<RestaurantCategoryWiseSale> list = await _restaurantCategoryWiseSaleService.GetByIdAsync(Model.Id);
            RestaurantCategoryWiseSale restaurantCategoryWiseSale = list.FirstOrDefault();
            restaurantCategoryWiseSale = _mapper.Map(Model, restaurantCategoryWiseSale);

            return Ok(_mapper.Map<RestaurantCategoryWiseSaleDTO>(await _restaurantCategoryWiseSaleService.UpdateCategoryWiseSale(_mapper.Map<RestaurantCategoryWiseSale>(restaurantCategoryWiseSale))));
        }

        [HttpDelete("CategoryWiseSale/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantCategoryWiseSaleDTO>>(await _restaurantCategoryWiseSaleService.ArchiveCategoryWiseSale(Id)));
        }

    }
}
