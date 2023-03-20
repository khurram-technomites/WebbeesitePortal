using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;
using WebAPI.Services.Domains;

namespace WebAPI.Controllers.Restaurant
{
	[Route("api/Restaurant")]
	[ApiController]
	[Authorize(Roles = "Admin , RestaurantOwner , RestaurantCashierStaff")]
	public class RestaurantTableReservationController : ControllerBase
	{
		private readonly IRestaurantTableService _RestaurantTableService;
		private readonly IRestaurantTableReservationService _restaurantTableReservationService;
		private readonly IMapper _mapper;
		public RestaurantTableReservationController(
			IRestaurantTableService restaurantTableService
			, IRestaurantTableReservationService restaurantTableReservationService
			, IMapper mapper
			)
		{
			_RestaurantTableService = restaurantTableService;
			_restaurantTableReservationService = restaurantTableReservationService;
			_mapper = mapper;
		}

		[Authorize(Roles = "Admin")]
		[HttpGet("TableReservation")]
		public async Task<IActionResult> GetAllAsync()
		{
			return Ok(new SuccessResponse<IEnumerable<RestaurantTableReservationDTO>>("Data received successfully", _mapper.Map<IEnumerable<RestaurantTableReservationDTO>>(await _restaurantTableReservationService.GetAllAsync())));
		}

		[HttpGet("TableReservation/Id")]
		public async Task<IActionResult> GetByIdAsync(long Id)
		{
			return Ok(new SuccessResponse<RestaurantTableReservationDTO>("Data received successfully", _mapper.Map<RestaurantTableReservationDTO>(await _restaurantTableReservationService.GetByIdAsync(Id))));
		}

		[HttpGet("GetByRestaurantId/{restaurantId}/TableReservation")]
		public async Task<IActionResult> GetByRestaurentTableId(long restaurantId)
		{
			return Ok(new SuccessResponse<IEnumerable<RestaurantTableReservationDTO>>("Data received successfully", _mapper.Map<IEnumerable<RestaurantTableReservationDTO>>(await _restaurantTableReservationService.GetByRestaurentTableId(restaurantId))));
		}

		[HttpPost("TableReservation")]
		public async Task<IActionResult> AddTableReservation(RestaurantTableReservationDTO Model)
		{
			var table = await _RestaurantTableService.GetByIdAsync(Model.RestaurantTableId);
			RestaurantTableReservation tableReservation = new RestaurantTableReservation();

			if (table.Status == Enum.GetName(typeof(Status), Status.Reserved))
			{
				return Ok(new ErrorResponse("Table already reserved", null));
			}
			else
			{
				Model.SeatsAvailable = table.Serving;
				Model.Status = Enum.GetName(typeof(Status), Status.Reserved);
				Model.RestaurantTable = null;
				tableReservation = await _restaurantTableReservationService.AddRestaurantTableReservationAsync(_mapper.Map<RestaurantTableReservation>(Model));
				tableReservation.RestaurantTable = table;

				await UpdateTableStatusAsync(table.Id, Enum.GetName(typeof(Status), Status.Reserved));

				return Ok(new SuccessResponse<RestaurantTableReservationDTO>("Table reserved successfully", _mapper.Map<RestaurantTableReservationDTO>(tableReservation)));
			}
		}

		[HttpPut("TableReservation")]
		public async Task<IActionResult> UpdateTableReservation(RestaurantTableReservationDTO Model)
		{
			RestaurantTableReservation restaurantTableReservation = await _restaurantTableReservationService.GetByIdAsync(Model.Id);
			restaurantTableReservation = _mapper.Map(Model, restaurantTableReservation);

			return Ok(new SuccessResponse<RestaurantTableReservationDTO>("Data received successfully", _mapper.Map<RestaurantTableReservationDTO>(await _restaurantTableReservationService.UpdateRestaurantTableReservationAsync(_mapper.Map<RestaurantTableReservation>(restaurantTableReservation)))));
		}

		

		[HttpDelete("TableReservation/{Id}")]
		public async Task<IActionResult> DeleteTableReservation(long Id)
		{
			var table = await _RestaurantTableService.GetByIdAsync(Id);
			RestaurantTableReservation tableReservation = new RestaurantTableReservation();

			if (table.Status != Enum.GetName(typeof(Status), Status.Reserved))
			{
				return Ok(new ErrorResponse("Table already unassigned", null));
			}
			else
			{
				await UpdateTableStatusAsync(table.Id, Enum.GetName(typeof(Status), Status.Active));

				var list = await _restaurantTableReservationService.GetReservedByRestaurentTableId(table.Id);
				var tableReservations = list.Where(x => x.Status == Enum.GetName(typeof(TableReservationStatus),TableReservationStatus.Reserved)).ToList();
				foreach (var item in tableReservations)
				{
					item.Status = Enum.GetName(typeof(Status), Status.Completed);

					await _restaurantTableReservationService.UpdateRestaurantTableReservationAsync(_mapper.Map<RestaurantTableReservation>(item));
				}
				tableReservation = tableReservations.LastOrDefault();

				return Ok(new SuccessResponse<RestaurantTableReservationDTO>("Table unassigned successfully", _mapper.Map<RestaurantTableReservationDTO>(tableReservation)));
			}
		}

		/* Private */

		private async Task UpdateTableStatusAsync(long tableId, string status)
		{
			var table = await _RestaurantTableService.GetByIdAsync(tableId);
			table.Status = status;
			await _RestaurantTableService.UpdateRestaurantTableAsync(table);
		}
	}
}
