using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Supplier
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Supplier")]
    public class SupplierCustomerController : ControllerBase
    {
		private readonly ICustomerService _customerService;
		private readonly ICustomerAddressService _customerAddressService;
		private readonly ISupplierCustomerService _restaurantCustomerservice;
		private readonly IMapper _mapper;
		private readonly IFTPUpload _fTPUpload;
		private readonly UserManager<AppUser> _userManager;

		public SupplierCustomerController(
			ICustomerService customerService
			, ICustomerAddressService customerAddressService
			, ISupplierCustomerService restaurantCustomerService
			, IMapper mapper
			, IFTPUpload fTPUpload
			, UserManager<AppUser> userManager
			)
		{
			_customerService = customerService;
			_customerAddressService = customerAddressService;
			_restaurantCustomerservice = restaurantCustomerService;
			_mapper = mapper;
			_fTPUpload = fTPUpload;
			_userManager = userManager;
		}
		[HttpGet("GetByRestaurant/{Id}")]
		public async Task<IActionResult> GetAllByBranch(long Id)
		{
			return Ok(_mapper.Map<IEnumerable<RestaurantCouponDTO>>(await _restaurantCustomerservice.GetSupplierCustomersAsync(Id)));
		}
		[HttpPost("Add")]
		public async Task<IActionResult> Add(RestaurantCouponDTO Model)
		{

				//restaurant customer
				var newCustomer = _mapper.Map<RestaurantCouponDTO>(await _restaurantCustomerservice.AddSupplierCustomer(_mapper.Map<RestaurantCoupon>(Model)));
				Model.RestaurantId = newCustomer.RestaurantId;
				Model.SupplierCouponId = newCustomer.SupplierCouponId;
		     	return Ok(Model);
		}
	}
}
