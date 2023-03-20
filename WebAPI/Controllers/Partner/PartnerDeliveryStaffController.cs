using AutoMapper;
using HelperClasses.DTOs.Order;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;
using WebAPI.Services;

namespace WebAPI.Controllers.Partner
{
	[Route("api/Partner/DeliveryStaff")]
	[ApiController]
	[Authorize(Roles = "RestaurantServiceStaff, RestaurantDeliveryStaff, RestaurantOwner, RestaurantCashierStaff")]
	public class PartnerDeliveryStaffController : ControllerBase
	{
		private readonly IRestaurantDeliveryStaffService _restaurantDeliveryStaffService;
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;
		private readonly UserManager<AppUser> _userManager;
		private readonly IFTPUpload _fTPUpload;

		public PartnerDeliveryStaffController(IRestaurantDeliveryStaffService restaurantDeliveryStaffService, IMapper mapper, UserManager<AppUser> userManager,
			IFTPUpload fTPUpload, IOrderService orderService)
		{
			_restaurantDeliveryStaffService = restaurantDeliveryStaffService;
			_mapper = mapper;
			_userManager = userManager;
			_fTPUpload = fTPUpload;
			_orderService = orderService;
		}

		[HttpGet("ByBranch/{BranchId}")]
		public async Task<IActionResult> GetByBranch(long BranchId)
		{
			return Ok(new SuccessResponse<IEnumerable<RestaurantDeliveryStaffDTO>>("", _mapper.Map<IEnumerable<RestaurantDeliveryStaffDTO>>(await _restaurantDeliveryStaffService.GetRestaurantDeliveryStaffByBranchAsync(BranchId))));
		}

		[Authorize(Roles = "RestaurantServiceStaff, RestaurantDeliveryStaff")]
		[HttpGet("ByUser")]
		public async Task<IActionResult> GetByID()
		{
			string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			RestaurantDeliveryStaff List = await _restaurantDeliveryStaffService.GetRestaurantDeliveryStaffByUserAsync(UserId);
			return Ok(new SuccessResponse<RestaurantDeliveryStaffDTO>("", _mapper.Map<RestaurantDeliveryStaffDTO>(List)));
		}

		[Authorize(Roles = "RestaurantServiceStaff, RestaurantDeliveryStaff")]
		[HttpPut]
		public async Task<IActionResult> Update(RestaurantDeliveryStaffDTO Model)
		{
			string UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			AppUser AppUser = await _userManager.FindByIdAsync(UserId);

			RestaurantDeliveryStaff deliveryStaff = await _restaurantDeliveryStaffService.GetRestaurantDeliveryStaffByUserAsync(UserId);

			if (!string.IsNullOrEmpty(Model.Logo) && !string.IsNullOrEmpty(deliveryStaff.Logo))
				_fTPUpload.DeleteFile(deliveryStaff.Logo);

			RestaurantDeliveryStaff staff = _mapper.Map(Model, deliveryStaff);

			if (Model.Logo is not null && Model.Logo.Contains("Draft"))
			{
				string LogoPath = "/Images/RestaurantDeliveryStaff/" + AppUser.UserName + "/";

				if (_fTPUpload.MoveFile(Model.Logo, ref LogoPath))
				{
					staff.Logo = LogoPath;
				}
			}

			staff.User = null;
			RestaurantDeliveryStaffDTO staffDTO = _mapper.Map<RestaurantDeliveryStaffDTO>(await _restaurantDeliveryStaffService.UpdateRestaurantDeliveryStaffAsync(staff));

			AppUser.FirstName = staffDTO.FirstName;
			AppUser.LastName = staffDTO.LastName;
			AppUser.Email = staffDTO.Email;
			AppUser.PhoneNumber = staffDTO.PhoneNumber;

			await _userManager.UpdateAsync(AppUser);

			return Ok(new SuccessResponse<RestaurantDeliveryStaffDTO>("", staffDTO));
		}

		[Authorize(Roles = "RestaurantServiceStaff, RestaurantDeliveryStaff")]
		[HttpPost("Order")]
		public async Task<IActionResult> GetOrders(HelperClasses.DTOs.RestaurantDeliveryStaff.RestaurantDeliveryStaffOrderFilterDTO Model)
		{
			return Ok(new SuccessResponse<IEnumerable<OrderDTO>>("", _mapper.Map<IEnumerable<OrderDTO>>(await _orderService.GetAllOrdersByRiderAsync(Model))));
		}
	}
}
