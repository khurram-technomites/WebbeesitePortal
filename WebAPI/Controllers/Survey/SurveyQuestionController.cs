using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Survey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.Services.Domains;

namespace WebAPI.Controllers.Survey
{
	[Route("api/[controller]")]
	[ApiController]
	public class SurveyQuestionController : ControllerBase
	{
		private readonly ISurveyQuestionService _service;
		private readonly IMapper _mapper;

		public SurveyQuestionController(ISurveyQuestionService service, IMapper mapper)
		{
			_service = service;
			_mapper = mapper;
		}

		[HttpGet("GetAll/Surveys/{surveyId}")]
		public async Task<IActionResult> GetAllBySurveyId(long surveyId)
		{
			return Ok(_mapper.Map<IEnumerable<SurveyQuestionDTO>>(await _service.GetAllAsyncBySurveyId(surveyId)));
		}

		//[HttpGet("GetAll")]
		//public async Task<IActionResult> GetAll()
		//{
		//    return Ok(_mapper.Map<IEnumerable<SurveyQuestionDTO>>(await _service.GetAllAsync()));
		//}

		[HttpGet("GetAll/Restaurant/{restaurantId}")]
		public async Task<IActionResult> GetAll(long restaurantId)
		{
			return Ok(_mapper.Map<IEnumerable<SurveyQuestionDTO>>(await _service.GetAllAsync(restaurantId)));
		}

		[HttpGet("{Id}")]
		public async Task<IActionResult> GetById(long Id)
		{
			IEnumerable<SurveyQuestionDTO> List = _mapper.Map<IEnumerable<SurveyQuestionDTO>>(await _service.GetByIdAsync(Id));
			return Ok(List.FirstOrDefault());
		}

		[HttpPost]
		public async Task<IActionResult> Add(SurveyQuestionDTO Model)
		{
			return Ok(_mapper.Map<SurveyQuestionDTO>(await _service.AddSurveyQuestionAsync(_mapper.Map<SurveyQuestion>(Model))));
		}

		[HttpPut]
		public async Task<IActionResult> Update(SurveyQuestionDTO Model)
		{
			IEnumerable<SurveyQuestion> surveQuestions = await _service.GetByIdAsync(Model.Id);
			SurveyQuestion currentModel = _mapper.Map(Model, surveQuestions.FirstOrDefault());

			currentModel.Restaurant = null;
			currentModel.Survey = null;

			return Ok(_mapper.Map<SurveyQuestionDTO>(await _service.UpdateSurveyQuestionAsync(currentModel)));
		}

		[HttpGet("ToggleStatus/{Id}")]
		public async Task<IActionResult> ToggleStatus(long Id)
		{
			IEnumerable<SurveyQuestion> survey = await _service.GetByIdAsync(Id);
			SurveyQuestion staff = survey.FirstOrDefault();

			if (staff.Status == Enum.GetName(typeof(Status), Status.Active))
				staff.Status = Enum.GetName(typeof(Status), Status.Inactive);
			else
				staff.Status = Enum.GetName(typeof(Status), Status.Active);

			return Ok(await _service.UpdateSurveyQuestionAsync(staff));
		}

		[HttpDelete("{Id}")]
		public async Task<IActionResult> Archive(long Id)
		{
			return Ok(_mapper.Map<SurveyQuestionDTO>(await _service.ArchiveSurveyQuestionAsync(Id)));
		}

	}
}
