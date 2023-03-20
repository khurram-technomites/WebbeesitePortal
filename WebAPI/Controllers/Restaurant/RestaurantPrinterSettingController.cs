using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantServiceStaff;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
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
	public class RestaurantPrinterSettingController : ControllerBase
	{
		private readonly IRestaurantPrinterSettingService _RestaurantPrinterSettingService;
		private readonly IMapper _mapper;

		public RestaurantPrinterSettingController(IRestaurantPrinterSettingService restaurantPrinterSettingService, IMapper mapper)
		{
			_RestaurantPrinterSettingService = restaurantPrinterSettingService;
			_mapper = mapper;
		}

		[HttpGet("PrinterSetting")]
		public async Task<IActionResult> GetAll()
		{
			var PrinterSetting = _mapper.Map<IEnumerable<RestaurantPrinterSettingDTO>>(await _RestaurantPrinterSettingService.GetAllAsync());
			return Ok(PrinterSetting);

		}
		[HttpGet("PrinterSetting/{Id}")]
		public async Task<IActionResult> GetById(long Id)
		{
			IEnumerable<RestaurantPrinterSettingDTO> PrinterSetting = _mapper.Map<IEnumerable<RestaurantPrinterSettingDTO>>(await _RestaurantPrinterSettingService.GetByIdAsync(Id));
			return Ok(PrinterSetting.FirstOrDefault());
		}

		[HttpGet("{Id}/PrinterSetting")]
		public async Task<IActionResult> GetByRestaurantIdAsync(long Id)
		{
			var PrinterSetting = _mapper.Map<IEnumerable<RestaurantPrinterSettingDTO>>(await _RestaurantPrinterSettingService.GetByRestaurantIdAsync(Id));
			return Ok(PrinterSetting);
		}

		[HttpGet("PrinterSetting/ByBranch/{Id}")]
		public async Task<IActionResult> GetByBranchIdAsync(long Id)
		{
			var PrinterSetting = _mapper.Map<IEnumerable<RestaurantPrinterSettingDTO>>(await _RestaurantPrinterSettingService.GetByRestaurantBranchIdAsync(Id));
			return Ok(PrinterSetting);
		}

		[HttpPost("PrinterSetting")]
		public async Task<IActionResult> AddPrinterSetting(RestaurantPrinterSettingDTO Model)
		{

			return Ok(_mapper.Map<RestaurantPrinterSettingDTO>(await _RestaurantPrinterSettingService.AddRestaurantPrinterSettingAsync(_mapper.Map<RestaurantPrinterSetting>(Model))));
		}

		[HttpPut("PrinterSetting")]
		public async Task<IActionResult> UpdatePrinterSetting(RestaurantPrinterSettingDTO Model)
		{
			IEnumerable<RestaurantPrinterSetting> List = await _RestaurantPrinterSettingService.GetByIdAsync(Model.Id);
			RestaurantPrinterSetting restaurantPrinterSetting = List.FirstOrDefault();
			restaurantPrinterSetting = _mapper.Map(Model, restaurantPrinterSetting);

			return Ok(_mapper.Map<RestaurantPrinterSettingDTO>(await _RestaurantPrinterSettingService.UpdateRestaurantPrinterSettingAsync(_mapper.Map<RestaurantPrinterSetting>(restaurantPrinterSetting))));
		}

		[HttpGet("PrinterSetting/ToggleStatus/{Id}")]
		public async Task<IActionResult> ToggleStatus(long Id)
		{
			IEnumerable<RestaurantPrinterSetting> PrinterSetting = await _RestaurantPrinterSettingService.GetByIdAsync(Id);
			RestaurantPrinterSetting Printer = PrinterSetting.FirstOrDefault();

			if (Printer.Status == Enum.GetName(typeof(Status), Status.Active))
				Printer.Status = Enum.GetName(typeof(Status), Status.Inactive);
			else
				Printer.Status = Enum.GetName(typeof(Status), Status.Active);

			return Ok(await _RestaurantPrinterSettingService.UpdateRestaurantPrinterSettingAsync(Printer));
		}
		[HttpDelete("PrinterSetting/{Id}")]
		public async Task<IActionResult> Delete(long Id)
		{
			return Ok(_mapper.Map<RestaurantPrinterSettingDTO>(await _RestaurantPrinterSettingService.ArchiveRestaurantPrinterSettingAsync(Id)));
		}
	}
}
