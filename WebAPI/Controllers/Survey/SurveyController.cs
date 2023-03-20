using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Survey;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Survey
{
	[Route("api/[controller]")]
	[Authorize(Roles = "Admin , RestaurantOwner , RestaurantCashierStaff")]
	[ApiController]
	public class SurveyController : ControllerBase
	{
		private readonly ISurveyService _surveyService;
		private readonly IMapper _mapper;

		public SurveyController(ISurveyService surveyService, IMapper mapper)
		{

			_surveyService = surveyService;
			_mapper = mapper;
		}

		[HttpGet("GetAll/Restaurants/{restaurantId}")]
		public async Task<IActionResult> GetAll(long restaurantId)
		{
			return Ok(new SuccessResponse<IEnumerable<SurveyDTO>>("Data received successfully", _mapper.Map<IEnumerable<SurveyDTO>>(await _surveyService.GetAllByRestaurantIdAsync(restaurantId))));
		}

		[HttpGet("GetAll/RestaurantBranches/{branchId}")]
		public async Task<IActionResult> GetAllByBranch(long branchId)
		{
			IEnumerable<SurveyDTO> result = _mapper.Map<IEnumerable<SurveyDTO>>(await _surveyService.GetAllByBranchIdAsync(branchId));

			try
			{
				result.ToList().ForEach(x =>
				{
					x.SurveyQuestions = x.SurveyQuestions.Where(x => x.Status == Enum.GetName(typeof(Status), Status.Active)).ToList();
				});
			}
			catch (Exception)
			{
			}
			
			return Ok(new SuccessResponse<IEnumerable<SurveyDTO>>("Data received successfully", result));
		}

		[HttpGet("{Id}")]
		public async Task<IActionResult> GetById(long Id)
		{
			IEnumerable<SurveyDTO> List = _mapper.Map<IEnumerable<SurveyDTO>>(await _surveyService.GetByIdAsync(Id));
			return Ok(new SuccessResponse<SurveyDTO>("Data received successfully", List.FirstOrDefault()));
		}

		[HttpPost]
		public async Task<IActionResult> Add(SurveyDTO Model)
		{
			return Ok(new SuccessResponse<SurveyDTO>("Data added successfully", _mapper.Map<SurveyDTO>(await _surveyService.AddSurveyAsync(_mapper.Map<Models.Survey>(Model)))));
		}

		[HttpPut]
		public async Task<IActionResult> Update(SurveyDTO Model)
		{
			return Ok(new SuccessResponse<SurveyDTO>("Data updated successfully", _mapper.Map<SurveyDTO>(await _surveyService.UpdateSurveyAsync(_mapper.Map<Models.Survey>(Model)))));
		}

		[HttpGet("ToggleStatus/{Id}")]
		public async Task<IActionResult> ToggleStatus(long Id)
		{
			IEnumerable<Models.Survey> survey = await _surveyService.GetByIdAsync(Id);
			Models.Survey staff = survey.FirstOrDefault();

			if (staff.Status == Enum.GetName(typeof(Status), Status.Active))
				staff.Status = Enum.GetName(typeof(Status), Status.Inactive);
			else
				staff.Status = Enum.GetName(typeof(Status), Status.Active);

			return Ok(await _surveyService.UpdateSurveyAsync(staff));
		}

		[HttpDelete("{Id}")]
		public async Task<IActionResult> Archive(long Id)
		{
			return Ok(new SuccessResponse<SurveyDTO>("Data added successfully", _mapper.Map<SurveyDTO>(await _surveyService.ArchiveSurveyAsync(Id))));
		}
	}
}
