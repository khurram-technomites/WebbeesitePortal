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
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Restaurant
{
	[Route("api/Restaurant/Customer")]
	[ApiController]
	[Authorize(Roles = "Admin , RestaurantOwner , RestaurantCashierStaff")]
	public class RestaurantCustomerController : ControllerBase
	{
		private readonly IRestaurantService _restaurantService;
		private readonly IRestaurantBranchService _restaurantBranchService;
		private readonly ICustomerService _customerService;
		private readonly ICustomerAddressService _customerAddressService;
		private readonly IRestaurantCustomerService _restaurantCustomerservice;
		private readonly IMapper _mapper;
		private readonly IFTPUpload _fTPUpload;
		private readonly UserManager<AppUser> _userManager;

		public RestaurantCustomerController(
			IRestaurantService restaurantService
			, IRestaurantBranchService restaurantBranchService
			, ICustomerService customerService
			, ICustomerAddressService customerAddressService
			, IRestaurantCustomerService restaurantCustomerService
			, IMapper mapper
			, IFTPUpload fTPUpload
			, UserManager<AppUser> userManager
			)
		{
			_restaurantService = restaurantService;
			_restaurantBranchService = restaurantBranchService;
			_customerService = customerService;
			_customerAddressService = customerAddressService;
			_restaurantCustomerservice = restaurantCustomerService;
			_mapper = mapper;
			_fTPUpload = fTPUpload;
			_userManager = userManager;
		}

		[HttpGet("GetByRestaurant/{Id}")]
		public async Task<IActionResult> GetAllByRestaurant(long Id)
		{
			return Ok(new SuccessResponse<IEnumerable<RestaurantCustomerDTO>>("Data received successfully", _mapper.Map<IEnumerable<RestaurantCustomerDTO>>(await _restaurantCustomerservice.GetRestaurantCustomersAsync(Id))));
		}

		[HttpGet("GetByRestaurant/{Id}/Contact/{contact}")]
		public async Task<IActionResult> GetAllByRestaurantSearchByContact(long Id, string contact = "")
		{
			return Ok(new SuccessResponse<IEnumerable<RestaurantCustomerDTO>>("Data received successfully", _mapper.Map<IEnumerable<RestaurantCustomerDTO>>(await _restaurantCustomerservice.GetRestaurantCustomersByContactAsync(Id, contact))));
		}

		[HttpGet("RestaurantBranches/{Id}")]
		public async Task<IActionResult> GetAllByBranch(long Id)
		{
			return Ok(new SuccessResponse<IEnumerable<RestaurantCustomerDTO>>("Data received successfully", _mapper.Map<IEnumerable<RestaurantCustomerDTO>>(await _restaurantCustomerservice.GetRestaurantBranchCustomersAsync(Id))));
		}

		[HttpPost]
		public async Task<IActionResult> Add(RestaurantCustomerDTO Model)
		{
			Customer existingCustomer = await _customerService.GetByContactAsync(Model.Customer.Contact);

			RestaurantCustomer restaurantCustomer = new();

			if (existingCustomer != null)
			{
				var existingRestaurantCustomer = await _restaurantCustomerservice.GetCustomerByRestaurantIdAsync(Model.RestaurantId, existingCustomer.Id);

				if (existingRestaurantCustomer == null)
				{
					restaurantCustomer.CustomerId = existingCustomer.Id;
					restaurantCustomer.RestaurantId = Model.RestaurantId;
					restaurantCustomer.RestaurantBranchId = Model.RestaurantBranchId;

					restaurantCustomer = await _restaurantCustomerservice.AddRestaurantCustomer(restaurantCustomer);
					restaurantCustomer.Customer = existingCustomer;
				}
				else
					return Ok(new ErrorResponse("Customer already exist", null));
			}
			else
			{
				IEnumerable<RestaurantBranch> branches = await _restaurantBranchService.GetRestaurantBranchById(Model.RestaurantBranchId);
				RestaurantBranch branch = branches.FirstOrDefault();

				//if (branch.Latitude != 0 && branch.Longitude != 0)
				//{
				//	foreach (var Address in Model.Customer.CustomerAddresses)
				//	{
				//		Address.Distance = DistanceHelper.DistanceTo((double)branch.Latitude, (double)branch.Longitude, (double)Address.Latitude, (double)Address.Longitude);
				//		Model.Customer.CustomerAddresses.FirstOrDefault(x => x.Id == Address.Id).Distance = Address.Distance;
				//	}
				//}
				Model.Customer.Status = Enum.GetName(typeof(Status), Status.Active);
				Model.Customer.IsActive = true;
				restaurantCustomer = await _restaurantCustomerservice.AddRestaurantCustomer(_mapper.Map<RestaurantCustomer>(Model));
			}

			return Ok(new SuccessResponse<RestaurantCustomerDTO>("Customer created successfully", _mapper.Map<RestaurantCustomerDTO>(restaurantCustomer)));
		}

	}
}
