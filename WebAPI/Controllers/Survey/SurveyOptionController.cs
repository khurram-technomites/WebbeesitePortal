using AutoMapper;
using HelperClasses.DTOs.Survey;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Survey
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyOptionController : ControllerBase
    {
        private readonly ISurveyOptionService _service;
        private readonly IMapper _mapper;

        public SurveyOptionController(ISurveyOptionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("GetAll/Surveys/{SurveyId}")]
        public async Task<IActionResult> GetAll(long SurveyId)
        {
            return Ok(_mapper.Map<IEnumerable<SurveyOptionDTO>>(await _service.GetAllBySurveyIdAsync(SurveyId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<SurveyOptionDTO> List = _mapper.Map<IEnumerable<SurveyOptionDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Add(SurveyOptionDTO Model)
        {
            return Ok(_mapper.Map<SurveyOptionDTO>(await _service.AddSurveyOptionAsync(_mapper.Map<SurveyOption>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(SurveyOptionDTO Model)
        {
            return Ok(_mapper.Map<SurveyOptionDTO>(await _service.UpdateSurveyOptionAsync(_mapper.Map<SurveyOption>(Model))));
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SurveyOptionDTO>(await _service.ArchiveSurveyOptionAsync(Id)));
        }
    }
}
