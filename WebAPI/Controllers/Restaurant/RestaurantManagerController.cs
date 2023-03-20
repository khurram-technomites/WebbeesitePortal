using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
	public class RestaurantManagerController : ControllerBase
	{
		private readonly IRestaurantManagerService _restaurantManagerService;
		private readonly IMapper _mapper;
		public RestaurantManagerController(IRestaurantManagerService restaurantManagerService, IMapper mapper)
		{
			_restaurantManagerService = restaurantManagerService;
			_mapper = mapper;
		}

		[HttpGet("Manager")]
		public async Task<IActionResult> GetAll()
		{
			var Manager = _mapper.Map<IEnumerable<RestaurantManagerDTO>>(await _restaurantManagerService.GetAllAsync());
			return Ok(Manager);
		}

		[HttpGet("Manager/{Id}")]
		public async Task<IActionResult> GetAllId(long Id)
		{
			var Manager = _mapper.Map<IEnumerable<RestaurantManagerDTO>>(await _restaurantManagerService.GetByIdAsync(Id));
			return Ok(Manager.FirstOrDefault());
		}

		[HttpGet("{Id}/Manager")]
		public async Task<IActionResult> GetByRestaurantIdAsync(long Id)
		{
			var Manager = _mapper.Map<IEnumerable<RestaurantManagerDTO>>(await _restaurantManagerService.GetByRestaurantIdAsync(Id));
			return Ok(Manager);
		}

		[HttpGet("Manager/ByBranch{Id}")]
		public async Task<IActionResult> GetByBranchIdAsync(long Id)
		{
			var Manager = _mapper.Map<IEnumerable<RestaurantManagerDTO>>(await _restaurantManagerService.GetByRestaurantBranchIdAsync(Id));
			return Ok(Manager);
		}

		[HttpPost("Manager")]
		public async Task<IActionResult> AddManager(RestaurantManagerDTO Model)
		{
			return Ok(_mapper.Map<RestaurantManagerDTO>(await _restaurantManagerService.AddRestaurantManagerAsync(_mapper.Map<RestaurantManager>(Model))));
		}

		[HttpPut("Manager")]
		public async Task<IActionResult> UpdateManager(RestaurantManagerDTO Model)
		{
			IEnumerable<RestaurantManager> list = await _restaurantManagerService.GetByIdAsync(Model.Id);
			RestaurantManager restaurantManager = list.FirstOrDefault();
			restaurantManager = _mapper.Map(Model, restaurantManager);

			return Ok(_mapper.Map<RestaurantManagerDTO>(await _restaurantManagerService.UpdateRestaurantManagerAsync(_mapper.Map<RestaurantManager>(restaurantManager))));
		}

		[HttpGet("Manager/ToggleStatus/{Id}")]
		public async Task<IActionResult> ToggleStatus(long Id)
		{
			IEnumerable<RestaurantManager> deliveryStaffs = await _restaurantManagerService.GetByIdAsync(Id);
			RestaurantManager staff = deliveryStaffs.FirstOrDefault();

			if (staff.Status == Enum.GetName(typeof(Status), Status.Active))
				staff.Status = Enum.GetName(typeof(Status), Status.Inactive);
			else
				staff.Status = Enum.GetName(typeof(Status), Status.Active);

			return Ok(await _restaurantManagerService.UpdateRestaurantManagerAsync(staff));
		}

		[HttpDelete("Manager/{Id}")]
		public async Task<IActionResult> Delete(long Id)
		{
			return Ok(_mapper.Map<RestaurantManagerDTO>(await _restaurantManagerService.ArchiveRestaurantManagerAsync(Id)));
		}

	}
}
