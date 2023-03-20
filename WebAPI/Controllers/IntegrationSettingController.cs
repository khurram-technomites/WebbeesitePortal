using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.Services.Domains;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class IntegrationSettingController : ControllerBase
    {
        private readonly IIntegrationSettingService _settingService;
        private readonly IMapper _mapper;
        public IntegrationSettingController(IIntegrationSettingService settingService, IMapper mapper)
        {
            _settingService = settingService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetIntegrationSettings()
        {
            return Ok(_mapper.Map<IEnumerable<IntegrationSettingDTO>>(await _settingService.GetAllAsync()));
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetIntegrationSettingByID(long Id)
        {
            IEnumerable<IntegrationSettingDTO> list = _mapper.Map<IEnumerable<IntegrationSettingDTO>>(await _settingService.GetIntegrationSettingByIdAsync(Id));
            return Ok(list.FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(IntegrationSettingDTO model)
        {
            return Ok(_mapper.Map<IntegrationSettingDTO>(await _settingService.AddIntegrationSettingAsync(_mapper.Map<IntegrationSetting>(model))));
        }
        [HttpPut]
        public async Task<IActionResult> PutEmailSettingAsync(IntegrationSettingDTO model)
        {
            IEnumerable<IntegrationSetting> list = await _settingService.GetIntegrationSettingByIdAsync(model.Id);
            IntegrationSetting integrationSetting = _mapper.Map(model, list.FirstOrDefault());            

            return Ok(_mapper.Map<IntegrationSettingDTO>(await _settingService.UpdateIntegrationSettingAsync(_mapper.Map<IntegrationSetting>(integrationSetting))));
        }
        

    }
}
