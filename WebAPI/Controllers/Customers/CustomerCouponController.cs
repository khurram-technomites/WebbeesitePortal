using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Customers
{
    [Route("api/Customer")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    public class CustomerCouponController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICouponService _couponService;
        private readonly ICustomerService _customerService;

        public CustomerCouponController(IMapper mapper, ICouponService couponService, ICustomerService customerService)
        {
            _mapper = mapper;
            _couponService = couponService;
            _customerService = customerService;
        }

        [HttpGet("Restaurant/{RestaurantId}/Coupon")]
        public async Task<IActionResult> GetAll(long RestaurantId)
        {
            string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IEnumerable<Customer> customers = await _customerService.GetByUserIdAsync(UserId);           
            long customerId = customers.FirstOrDefault().Id;

            return Ok(new SuccessResponse<IEnumerable<CouponDTO>>("", _mapper.Map<IEnumerable<CouponDTO>>(await _couponService.GetByCustomerAsync(RestaurantId, customerId))));
        }
    }
}
