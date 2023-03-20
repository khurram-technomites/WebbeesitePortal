using AutoMapper;
using HelperClasses.DTOs.Aggregators;
using HelperClasses.DTOs.CurrencyNote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;
using WebAPI.Services.Domains;

namespace WebAPI.Controllers.CurrencyNotes
{
	[Route("api/CurrencyNote")]
	[ApiController]
	[Authorize(Roles = "Admin , RestaurantOwner , RestaurantCashierStaff")]
	public class CardSchemeController : ControllerBase
	{
		private readonly ICurrencyNoteService _currencyNoteService;
		private readonly IMapper _mapper;

		public CardSchemeController(ICurrencyNoteService currencyNoteService, IMapper mapper)
		{
			_currencyNoteService = currencyNoteService;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllasync()
		{
			return Ok(new SuccessResponse<IEnumerable<CurrencyNoteDTO>>("Data received successfully", _mapper.Map<IEnumerable<CurrencyNoteDTO>>(await _currencyNoteService.GetAllAsync())));
		}

		[HttpGet("{Id}")]
		public async Task<IActionResult> GetById(long Id)
		{
			return Ok(new SuccessResponse<IEnumerable<CurrencyNoteDTO>>("Data received successfully", _mapper.Map<IEnumerable<CurrencyNoteDTO>>(await _currencyNoteService.GetByIdAsync(Id))));
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> AddCurrencyNote(CurrencyNoteDTO Model)
		{
			return Ok(new SuccessResponse<CurrencyNoteDTO>("Data added successfully", _mapper.Map<CurrencyNoteDTO>(await _currencyNoteService.AddCurrencyNote(_mapper.Map<CurrencyNote>(Model)))));
		}

		[Authorize(Roles = "Admin")]
		[HttpPut]
		public async Task<IActionResult> UpdateCurrencyNote(CurrencyNoteDTO Model)
		{
			IEnumerable<CurrencyNote> list = await _currencyNoteService.GetByIdAsync(Model.Id);
			CurrencyNote currencyNoteDTO = list.FirstOrDefault();
			currencyNoteDTO = _mapper.Map(Model, currencyNoteDTO);

			return Ok(new SuccessResponse<CurrencyNoteDTO>("Data updated successfully", _mapper.Map<CurrencyNoteDTO>(await _currencyNoteService.UpdateCurrencyNote(_mapper.Map<CurrencyNote>(currencyNoteDTO)))));
		}

		[Authorize(Roles = "Admin")]
		[HttpDelete("{Id}")]
		public async Task<IActionResult> Delete(long Id)
		{
			return Ok(new SuccessResponse<CurrencyNoteDTO>("Data deleted successfully", _mapper.Map<CurrencyNoteDTO>(await _currencyNoteService.ArchiveCurrencyNote(Id))));
		}
	}
}
