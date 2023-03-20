using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage/BranchBusinessSetting")]
    [ApiController]
    public class GarageBranchBusinessSettingController : ControllerBase
    {

        private readonly IGarageBranchBusinessSettingService _settingService;
        private readonly IMapper _mapper;
        public GarageBranchBusinessSettingController(IGarageBranchBusinessSettingService settingService, IMapper mapper)
        {
            _settingService = settingService;
            _mapper = mapper;
        }
        [HttpGet("GetAll/Business/{BusinessId}")]
        public async Task<IActionResult> GetBusinessSettings(long BusinessId)
        {
            return Ok(_mapper.Map<IEnumerable<GarageBranchBusinessSettingDTO>>(await _settingService.GetBranchBusinessSettingByBusinessIdAsync(BusinessId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBusinessSettingByID(long Id)
        {
            IEnumerable<GarageBranchBusinessSettingDTO> list = _mapper.Map<IEnumerable<GarageBranchBusinessSettingDTO>>(await _settingService.GetBranchBusinessSettingByIdAsync(Id));
            return Ok(list.FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(GarageBranchBusinessSettingDTO model)
        {
            return Ok(_mapper.Map<GarageBranchBusinessSettingDTO>(await _settingService.AddBranchBusinessSettingAsync(_mapper.Map<GarageBranchBusinessSetting>(model))));
        }
        [HttpPut]
        public async Task<IActionResult> PutAsync(GarageBranchBusinessSettingDTO model)
        {
            return Ok(_mapper.Map<GarageBranchBusinessSettingDTO>(await _settingService.UpdateBranchBusinessSettingAsync(_mapper.Map<GarageBranchBusinessSetting>(model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageCareerDTO>(await _settingService.ArchiveBranchBusinessSettingAsync(Id)));
        }

    }
}
