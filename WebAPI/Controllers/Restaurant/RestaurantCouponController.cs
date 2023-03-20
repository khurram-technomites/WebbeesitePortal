using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/RestaurantCoupon")]
    [ApiController]
    public class RestaurantCouponController : ControllerBase
    {
        private readonly IRestaurantCouponService _service;
        private readonly IMapper _mapper;
        public RestaurantCouponController(IRestaurantCouponService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("Restaurant/{RestaurantId}/Coupons/{CouponId}")]
        public async Task<IActionResult> GetAll(long RestaurantId, long CouponId)
        {
            IEnumerable<RestaurantCouponDTO> RestaurantCoupon = _mapper.Map<IEnumerable<RestaurantCouponDTO>>(await _service.GetCoupon(RestaurantId, CouponId));
            return Ok(RestaurantCoupon.FirstOrDefault());
        }

        [HttpGet("Coupons/{CouponId}")]
        public async Task<IActionResult> GetById(long CouponId)
        {
            IEnumerable<RestaurantCouponDTO> List = _mapper.Map<IEnumerable<RestaurantCouponDTO>>(await _service.GetCouponsByCouponID(CouponId));
            return Ok(List);
        }

        [HttpGet("Restaurant/{RestaurantId}")]
        public async Task<IActionResult> GetByRestaurantCoupon(long RestaurantId)
        {
            IEnumerable<SupplierCouponDTO> List = _mapper.Map<IEnumerable<SupplierCouponDTO>>(await _service.GetRestaurantCoupons(RestaurantId));
            return Ok(List);
        }

        [HttpPost]
        public async Task<IActionResult> Add(RestaurantCouponDTO Model)
        {
            Model.Restaurant = null;
            Model.SupplierCoupon = null;
            return Ok(_mapper.Map<RestaurantCouponDTO>(await _service.AddRestaurantCouponAsync(_mapper.Map<RestaurantCoupon>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(RestaurantCouponDTO Model)
        {
            Model.Restaurant = null;
            Model.SupplierCoupon = null;
            return Ok(_mapper.Map<RestaurantCouponDTO>(await _service.UpdateRestaurantCouponAsync(_mapper.Map<RestaurantCoupon>(Model))));
        }

        //[HttpDelete("{Id}")]
        //public async Task<IActionResult> Archive(long Id)
        //{
        //    return Ok(_mapper.Map<RestaurantCouponDTO>(await _service.ArchiveRestaurantCouponAsync(Id)));
        //} uzaif
        [HttpDelete("{Id}")]
        public IActionResult RestaurantCoupon(long Id)
        {
            return Ok(_service.DeleteRestaurantCouponAsync(Id));
        }
    }
}
