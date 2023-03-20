using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;
using HelperClasses.DTOs.RestaurantCashierStaff;
using System.Security.Claims;
using System.Linq;
using WebAPI.Helpers;

namespace WebAPI.Controllers.Restaurant
{
	[Route("api/Restaurant")]
	[ApiController]
	[Authorize(Roles = "Admin , RestaurantOwner , RestaurantCashierStaff")]
	public class RestaurantReservationController : ControllerBase
	{
		private readonly IRestaurantReservationService _restaurantReservationService;
		private readonly IRestaurantCashierStaffService _RestaurantCashierStaffService;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
		public RestaurantReservationController(
			IRestaurantReservationService restaurantReservationService
			, IRestaurantCashierStaffService restaurantCashierStaffService
			, IUserService userService
			, IMapper mapper
			)
		{
			_restaurantReservationService = restaurantReservationService;
			_RestaurantCashierStaffService = restaurantCashierStaffService;
			_userService = userService;
			_mapper = mapper;
		}

		#region Status Repsonse Apis

		[Authorize(Roles = "Admin")]
		[HttpGet("Reservation")]
		public async Task<IActionResult> GetAll()
		{
			return Ok(new SuccessResponse<IEnumerable<RestaurantReservationDTO>>("Data received successfully", _mapper.Map<IEnumerable<RestaurantReservationDTO>>(await _restaurantReservationService.GetAllAsync())));
		}

		[HttpGet("Reservation/{Id}")]
		public async Task<IActionResult> GetById(long Id)
		{
			return Ok(new SuccessResponse<RestaurantReservationDTO>("Data by Id received successfully", _mapper.Map<RestaurantReservationDTO>(await _restaurantReservationService.GetByIdAsync(Id))));
		}

		[HttpGet("GetByRestaurantId/{restaurantId}/Reservation")]
		public async Task<IActionResult> GetByRestaurantId(long restaurantId)
		{
			return Ok(new SuccessResponse<IEnumerable<RestaurantReservationDTO>>("Data by restaurant received successfully", _mapper.Map<IEnumerable<RestaurantReservationDTO>>(await _restaurantReservationService.GetByRestaurantIdAsync(restaurantId))));
		}

		[HttpGet("GetByRestaurantBranchId/{branchId}/Reservation")]
		public async Task<IActionResult> GetByBranchId(long branchId)
		{
			return Ok(new SuccessResponse<IEnumerable<RestaurantReservationDTO>>("Data by branch received successfully", _mapper.Map<IEnumerable<RestaurantReservationDTO>>(await _restaurantReservationService.GetByRestaurantBranchIdAsync(branchId))));
		}

		[HttpGet("GetReservedByRestaurantBranchId/{branchId}/Reservation")]
		public async Task<IActionResult> GetReservedByBranchId(long branchId)
		{
			return Ok(new SuccessResponse<IEnumerable<RestaurantReservationDTO>>("Data by branch received successfully", _mapper.Map<IEnumerable<RestaurantReservationDTO>>(await _restaurantReservationService.GetByStatusAndRestaurantBranchIdAsync(branchId, Enum.GetName(typeof(ReservationStatus), ReservationStatus.Reserved)))));
		}

		[HttpPost("Reservation")]
		public async Task<IActionResult> AddReservation(RestaurantReservationDTO Model)
		{
			RestaurantCashierStaffDTO cashier = await GetCurrentStaff();

			Model.Status = Enum.GetName(typeof(ReservationStatus), ReservationStatus.Reserved);
			Model.RestaurantCashierStaffId = cashier.Id;
			Model.RestaurantBranchId = cashier.RestaurantBranchId;
			Model.RestaurantId = cashier.RestaurantId;

			if (Model.ReservationTimeDate.HasValue)
				Model.ReservationTime = Model.ReservationTimeDate.Value.TimeOfDay;

			Model.ReservationDate = Model.ReservationDate.ToDubaiDateTime();
			Model.ReservationTime = Model.ReservationTime.ToDubaiDateTime();

			return Ok(new SuccessResponse<RestaurantReservationDTO>("Data created successfully", _mapper.Map<RestaurantReservationDTO>(await _restaurantReservationService.AddRestaurantReservationAsync(_mapper.Map<RestaurantReservation>(Model)))));
		}

		[HttpPut("Reservation")]
		public async Task<IActionResult> UpdateReservation(RestaurantReservationDTO Model)
		{
			RestaurantReservation restaurantReservation = await _restaurantReservationService.GetByIdAsync(Model.Id);
			RestaurantCashierStaffDTO cashier = await GetCurrentStaff();

			Model.RestaurantCashierStaffId = cashier.Id;
			Model.RestaurantBranchId = cashier.RestaurantBranchId;
			Model.RestaurantId = cashier.RestaurantId;
			restaurantReservation = _mapper.Map(Model, restaurantReservation);

			if (Model.ReservationTimeDate.HasValue)
				Model.ReservationTime = Model.ReservationTimeDate.Value.TimeOfDay;

			Model.ReservationDate = Model.ReservationDate.ToDubaiDateTime();
			Model.ReservationTime = Model.ReservationTime.ToDubaiDateTime();


			return Ok(new SuccessResponse<RestaurantReservationDTO>("Data created successfully", _mapper.Map<RestaurantReservationDTO>(await _restaurantReservationService.UpdateRestaurantReservationAsync(_mapper.Map<RestaurantReservation>(restaurantReservation)))));
		}

		[HttpGet("Reservation/Search/{Contact}")]
		public async Task<IActionResult> Search(string Contact)
		{
			var data = await _restaurantReservationService.GetByContactAsync(Contact);
			
			if (data == null)
				return Ok(new ErrorResponse("Reservation not found", null));

			if (data.Status != Enum.GetName(typeof(ReservationStatus), ReservationStatus.Reserved))
			{
				object result = new
				{
					Name = data.Name,
					Contact = data.Contact,
					Time = data.ReservationTime,
					Status = data.Status,

					IsCompleted = data.Status == Enum.GetName(typeof(ReservationStatus), ReservationStatus.Completed) ? true : false,
					IsExpired = data.Status == Enum.GetName(typeof(ReservationStatus), ReservationStatus.Expired) ? true : false,
					IsCanceled = data.Status == Enum.GetName(typeof(ReservationStatus), ReservationStatus.Canceled) ? true : false,
				};

				if (data.Status == Enum.GetName(typeof(ReservationStatus), ReservationStatus.Completed))
				{
					return Ok(new ErrorResponse("Reservation Has Completed", result));
				}
				else if (data.Status == Enum.GetName(typeof(ReservationStatus), ReservationStatus.Expired))
				{
					return Ok(new ErrorResponse("Reservation Has Expired", result));
				}
				else if (data.Status == Enum.GetName(typeof(ReservationStatus), ReservationStatus.Canceled))
				{
					return Ok(new ErrorResponse("Reservation Has Canceled", result));
				}
				else
				{
					return Ok(new ErrorResponse("Reservation not found", null));
				}
			}
			else if (data.ReservationDate.Date < DateTime.UtcNow.Date)
			{
				data.Status = Enum.GetName(typeof(ReservationStatus), ReservationStatus.Expired);

				//update old reservations
				await _restaurantReservationService.UpdateRestaurantReservationAsync(data);

				object result = new
				{
					Name = data.Name,
					Contact = data.Contact,
					Time = data.ReservationTime,
					Status = Enum.GetName(typeof(ReservationStatus), ReservationStatus.Expired),

					IsCompleted = data.Status == Enum.GetName(typeof(ReservationStatus), ReservationStatus.Completed) ? true : false,
					IsExpired = data.Status == Enum.GetName(typeof(ReservationStatus), ReservationStatus.Expired) ? true : false,
					IsCanceled = data.Status == Enum.GetName(typeof(ReservationStatus), ReservationStatus.Canceled) ? true : false,
				};

				return Ok(new ErrorResponse("Reservation Has Expired", result));
			}

			return Ok(new SuccessResponse<RestaurantReservationDTO>("Data received successfully", _mapper.Map<RestaurantReservationDTO>(await _restaurantReservationService.UpdateRestaurantReservationAsync(_mapper.Map<RestaurantReservation>(data)))));
		}

		[HttpGet("Reservation/{Id}/Complete")]
		public async Task<IActionResult> ToggleStatus(long Id)
		{
			RestaurantReservation data = await _restaurantReservationService.GetByIdAsync(Id);

			//if (data.Status == Enum.GetName(typeof(ReservationStatus), ReservationStatus.Reserved))
			data.Status = Enum.GetName(typeof(ReservationStatus), ReservationStatus.Completed);
			//else
			//    data.Status = Enum.GetName(typeof(ReservationStatus), ReservationStatus.Reserved);

			return Ok(new SuccessResponse<RestaurantReservationDTO>("Data updated successfully", _mapper.Map<RestaurantReservationDTO>(await _restaurantReservationService.UpdateRestaurantReservationAsync(_mapper.Map<RestaurantReservation>(data)))));
		}

		[HttpDelete("Reservation/{Id}")]
		public async Task<IActionResult> DeleteReservation(long Id)
		{
			RestaurantReservation data = await _restaurantReservationService.GetByIdAsync(Id);

			//if (data.Status == Enum.GetName(typeof(ReservationStatus), ReservationStatus.Reserved))
			data.Status = Enum.GetName(typeof(ReservationStatus), ReservationStatus.Canceled);
			//else
			//    data.Status = Enum.GetName(typeof(ReservationStatus), ReservationStatus.Reserved);

			return Ok(new SuccessResponse<RestaurantReservationDTO>("Data canceled successfully", _mapper.Map<RestaurantReservationDTO>(await _restaurantReservationService.UpdateRestaurantReservationAsync(_mapper.Map<RestaurantReservation>(data)))));
		}


		//get cashier staff
		private async Task<RestaurantCashierStaffDTO> GetCurrentStaff()
		{
			string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			IEnumerable<AppUser> list = await _userService.GetUsersByIdAsync(userId);
			AppUser appUser = list.FirstOrDefault();

			RestaurantCashierStaffDTO user = new();

			if (appUser.LoginFor == Enum.GetName(typeof(Logins), Logins.RestaurantCashierStaff))
			{
				user = _mapper.Map<RestaurantCashierStaffDTO>(_RestaurantCashierStaffService.GetRestaurantCashierStaffByUserAsync(appUser.Id).Result);
			}

			return user;
		}
		#endregion
	}
}
