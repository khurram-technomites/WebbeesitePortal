using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.CustomerFeedback;
using HelperClasses.DTOs.ServiceStaff;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Models;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WebAPI.ResponseWrapper;
using System.Security.Claims;
using HelperClasses.DTOs.RestaurantServiceStaff;
using Microsoft.AspNetCore.Authorization;
using HelperClasses.DTOs.RestaurantCashierStaff;
using HelperClasses.DTOs.RestaurantKitchenManager;
using HelperClasses.DTOs.Restaurant;

namespace WebAPI.Controllers.CustomerFeedback
{
	[Route("api/[controller]")]
    [Authorize(Roles = "Admin , RestaurantOwner , RestaurantCashierStaff")]
	[ApiController]
	public class CustomerFeedbackController : ControllerBase
	{
		private readonly ICustomerFeedbackService _service;
		private readonly IUserService _userService;
		private readonly IRestaurantServiceStaffService _restaurantServiceStaffService;
		private readonly IRestaurantDeliveryStaffService _restaurantDeliveryStaffService;
		private readonly IRestaurantCashierStaffService _restaurantCashierStaffService;
		private readonly IRestaurantKitchenManagerService _restaurantKitchenManagerService;
		private readonly IMapper _mapper;

		public CustomerFeedbackController(
			ICustomerFeedbackService customerFeedbackService
			, IUserService userService
			, IRestaurantServiceStaffService restaurantServiceStaffService
			, IRestaurantDeliveryStaffService restaurantDeliveryStaffService
			, IRestaurantCashierStaffService restaurantCashierStaffService
			, IRestaurantKitchenManagerService restaurantKitchenManagerService
			, IMapper mapper
			)
		{
			_service = customerFeedbackService;
			_userService = userService;
			_restaurantServiceStaffService = restaurantServiceStaffService;
			_restaurantDeliveryStaffService = restaurantDeliveryStaffService;
			_restaurantCashierStaffService = restaurantCashierStaffService;
			_restaurantKitchenManagerService = restaurantKitchenManagerService;
			_mapper = mapper;
		}

		[HttpPost("GetAll")]
		public async Task<IActionResult> GetAll(PagingParameters Paging)
		{
			return Ok(new SuccessResponse<IEnumerable<CustomerFeedbackDTO>>("Data received successfully", _mapper.Map<IEnumerable<CustomerFeedbackDTO>>(await _service.GetAllAsync(Paging))));
		}

		[HttpGet("{Id}")]
		public async Task<IActionResult> GetById(long Id)
		{
			CustomerFeedbackDTO data = _mapper.Map<CustomerFeedbackDTO>(await _service.GetByIdAsync(Id));

			return Ok(new SuccessResponse<CustomerFeedbackDTO>("Data received successfully", data));
		}

		[HttpGet("Restaurant/{Id}")]
		public async Task<IActionResult> GetAllByRestaurantId(long Id)
		{
			IEnumerable<CustomerFeedbackDTO> data = _mapper.Map<IEnumerable<CustomerFeedbackDTO>>(await _service.GetAllByRestaurantIdAsync(Id));

			return Ok(new SuccessResponse<IEnumerable<CustomerFeedbackDTO>>("Data received successfully", data));
		}

		[HttpGet("RestaurantBranch/{Id}")]
		public async Task<IActionResult> GetAllByRestaurantBranchId(long Id)
		{
			IEnumerable<CustomerFeedbackDTO> data = _mapper.Map<IEnumerable<CustomerFeedbackDTO>>(await _service.GetAllByBranchIdAsync(Id));

			return Ok(new SuccessResponse<IEnumerable<CustomerFeedbackDTO>>("Data received successfully", data));
		}

		[HttpPost("")]
		public async Task<IActionResult> Add(CustomerFeedbackDTO Model)
		{
			Model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			IEnumerable<AppUser> list = await _userService.GetUsersByIdAsync(Model.UserId);
			AppUser appUser = list.FirstOrDefault();

			Model.UserType = appUser.LoginFor;
			if (Model.UserType == Enum.GetName(typeof(Logins), Logins.RestaurantServiceStaff))
			{
				var usr = _mapper.Map<RestaurantServiceStaffDTO>(_restaurantServiceStaffService.GetRestaurantServiceStaffByUserAsync(appUser.Id).Result.FirstOrDefault());
				Model.UserDetailId = usr.Id;
				Model.RestaurantId = usr.RestaurantId;
				Model.RestaurantBranchId = usr.RestaurantBranchId;
            }
            else if (Model.UserType == Enum.GetName(typeof(Logins), Logins.RestaurantDeliveryStaff))
			{
				var usr = _mapper.Map<RestaurantDeliveryStaffDTO>(_restaurantDeliveryStaffService.GetRestaurantDeliveryStaffByUserAsync(appUser.Id).Result);
				Model.UserDetailId = usr.Id;
				Model.RestaurantId = usr.RestaurantId;
				Model.RestaurantBranchId = usr.RestaurantBranchId;
            }
            else if (Model.UserType == Enum.GetName(typeof(Logins), Logins.RestaurantCashierStaff))
			{
				var usr = _mapper.Map<RestaurantCashierStaffDTO>(_restaurantCashierStaffService.GetRestaurantCashierStaffByUserAsync(appUser.Id).Result);
				Model.UserDetailId = usr.Id;
				Model.RestaurantId = usr.RestaurantId;
				Model.RestaurantBranchId = usr.RestaurantBranchId;
			}
			else if (Model.UserType == Enum.GetName(typeof(Logins), Logins.RestaurantKitchenManager))
			{
				var usr = _mapper.Map<RestaurantKitchenManagerDTO>(_restaurantKitchenManagerService.GetRestaurantKitchenManagerByUserAsync(appUser.Id).Result);
				Model.UserDetailId = usr.Id;
				Model.RestaurantId = usr.RestaurantId;
				Model.RestaurantBranchId = usr.RestaurantBranchId;
            }
			Model.ActiveStatus = Enum.GetName(typeof(Status), Status.Active);
            var result = _mapper.Map<CustomerFeedbackDTO>(await _service.AddAsync(_mapper.Map<Models.CustomerFeedback>(Model)));

			return Ok(new SuccessResponse<CustomerFeedbackDTO>("Data added successfully", result));
		}

		[HttpPut("")]
		public async Task<IActionResult> Update(CustomerFeedbackDTO Model)
		{
			Models.CustomerFeedback data = await _service.GetByIdAsync(Model.Id);
			Models.CustomerFeedback currentModel = _mapper.Map(Model, data);

			var result = _mapper.Map<CustomerFeedbackDTO>(await _service.UpdateAsync(currentModel));

			return Ok(new SuccessResponse<CustomerFeedbackDTO>("Data updated successfully", result));
		}

		[HttpDelete("{Id}")]
		public async Task<IActionResult> Delete(long Id)
		{
			var result = _mapper.Map<CustomerFeedbackDTO>(await _service.ArchiveAsync(Id));
			return Ok(new SuccessResponse<CustomerFeedbackDTO>("Data deleted successfully", result));
		}

		[HttpGet("ToggleStatus/{Id}")]
		public async Task<IActionResult> ToggleStatus(long Id)
		{
			Models.CustomerFeedback data = await _service.GetByIdAsync(Id);

			if (data.Status == Enum.GetName(typeof(Status), Status.Active))
				data.Status = Enum.GetName(typeof(Status), Status.Inactive);
			else
				data.Status = Enum.GetName(typeof(Status), Status.Active);

			var result = _mapper.Map<CustomerFeedbackDTO>(await _service.UpdateAsync(data));
			return Ok(new SuccessResponse<CustomerFeedbackDTO>("Data deleted successfully", result));
		}
	}
}
