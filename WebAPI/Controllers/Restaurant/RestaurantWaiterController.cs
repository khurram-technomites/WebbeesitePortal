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
using WebAPI.ResponseWrapper;
using WebAPI.Services.Domains;

namespace WebAPI.Controllers.Restaurant
{
	[Route("api/Restaurant")]
	[ApiController]
	public class RestaurantWaiterController : ControllerBase
	{
		private readonly IRestaurantWaiterService _RestaurantWaiterService;
		private readonly IMapper _mapper;
		public RestaurantWaiterController(IRestaurantWaiterService RestaurantWaiterService, IMapper mapper)
		{
			_RestaurantWaiterService = RestaurantWaiterService;
			_mapper = mapper;
		}

		[HttpGet("Waiter")]
		public async Task<IActionResult> GetAll()
		{
			var Waiter = _mapper.Map<IEnumerable<RestaurantWaiterDTO>>(await _RestaurantWaiterService.GetAllAsync());
			return Ok(Waiter);
		}

		[HttpGet("Waiter/{Id}")]
		public async Task<IActionResult> GetAllId(long Id)
		{
			var Waiter = _mapper.Map<IEnumerable<RestaurantWaiterDTO>>(await _RestaurantWaiterService.GetByIdAsync(Id));
			return Ok(Waiter.FirstOrDefault());
		}

		[HttpGet("{Id}/Waiter")]
		public async Task<IActionResult> GetByRestaurantIdAsync(long Id)
		{
			var Waiter = _mapper.Map<IEnumerable<RestaurantWaiterDTO>>(await _RestaurantWaiterService.GetByRestaurantIdAsync(Id));
			return Ok(Waiter);
		}

		[HttpGet("Waiter/ByBranch/{Id}")]
		public async Task<IActionResult> GetByBranchIdAsync(long Id)
		{
			var Waiter = _mapper.Map<IEnumerable<RestaurantWaiterDTO>>(await _RestaurantWaiterService.GetByRestaurantBranchIdAsync(Id));

			return Ok(new SuccessResponse<IEnumerable<RestaurantWaiterDTO>>("Data by branch received successfully", _mapper.Map<IEnumerable<RestaurantWaiterDTO>>(Waiter)));
		}

		[HttpPost("Waiter")]
		public async Task<IActionResult> AddWaiter(RestaurantWaiterDTO Model)
		{
			return Ok(_mapper.Map<RestaurantWaiterDTO>(await _RestaurantWaiterService.AddRestaurantWaiterAsync(_mapper.Map<RestaurantWaiter>(Model))));
		}

		[HttpPut("Waiter")]
		public async Task<IActionResult> UpdateWaiter(RestaurantWaiterDTO Model)
		{
			IEnumerable<RestaurantWaiter> list = await _RestaurantWaiterService.GetByIdAsync(Model.Id);
			RestaurantWaiter RestaurantWaiter = list.FirstOrDefault();
			RestaurantWaiter = _mapper.Map(Model, RestaurantWaiter);

			return Ok(_mapper.Map<RestaurantWaiterDTO>(await _RestaurantWaiterService.UpdateRestaurantWaiterAsync(_mapper.Map<RestaurantWaiter>(RestaurantWaiter))));
		}

        [HttpGet("Waiter/ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
		{
            IEnumerable<RestaurantWaiter> WaiterStaffs = await _RestaurantWaiterService.GetByIdAsync(Id);
            RestaurantWaiter staff = WaiterStaffs.FirstOrDefault();

            if (staff.Status == Enum.GetName(typeof(Status), Status.Active))
                staff.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                staff.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _RestaurantWaiterService.UpdateRestaurantWaiterAsync(staff));
        }

        [HttpDelete("Waiter/{Id}")]
		public async Task<IActionResult> Delete(long Id)
		{
			return Ok(_mapper.Map<RestaurantWaiterDTO>(await _RestaurantWaiterService.ArchiveRestaurantWaiterAsync(Id)));
		}


	}
}
