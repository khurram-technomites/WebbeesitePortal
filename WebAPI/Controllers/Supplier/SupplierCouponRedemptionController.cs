using AutoMapper;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Supplier
{
    [Route("api/SupplierCouponRedemption")]
    [ApiController]
    public class SupplierCouponRedemptionController : ControllerBase
    {
        private readonly ISupplierCouponRedemptionService _redemptionService;
        private readonly IMapper _mapper;


        public SupplierCouponRedemptionController(ISupplierCouponRedemptionService redemptionService, IMapper mapper)
        {
            _redemptionService = redemptionService;
            _mapper = mapper;
        }

        [HttpGet("GetAll/Restaurant/{RestaurantId}/Coupon/{SupplierCouponId}")]
        public async Task<IActionResult> GetAllByRestaurant(string RestaurantId , long SupplierCouponId )

        {
            return Ok(_mapper.Map<IEnumerable<SupplierCouponRedemptionDTO>>(await _redemptionService.GetCouponRedemptionByRestaurant(SupplierCouponId,RestaurantId)));
        }

        [HttpGet("GetAll/Coupons/{SupplierCouponId}")]
        public async Task<IActionResult> GetAll(long SupplierCouponId)

        {
            return Ok(_mapper.Map<IEnumerable<SupplierCouponRedemptionDTO>>(await _redemptionService.GetCouponRedemption(SupplierCouponId)));
        }

        [HttpPost]
        public async Task<IActionResult> Add(SupplierCouponRedemptionDTO Model)
        {
            return Ok(_mapper.Map<SupplierCouponRedemptionDTO>(await _redemptionService.AddSupplierCouponRedemption(_mapper.Map<SupplierCouponRedemption>(Model))));
        }

    }
}
