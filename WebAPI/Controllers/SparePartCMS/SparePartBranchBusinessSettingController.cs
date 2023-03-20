using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SparePart/BranchBusinessSetting")]
    [ApiController]
    public class SparePartBranchBusinessSettingController : ControllerBase
    {

        private readonly ISparePartBranchBusinessSettingService _settingService;
        private readonly IMapper _mapper;
        public SparePartBranchBusinessSettingController(ISparePartBranchBusinessSettingService settingService, IMapper mapper)
        {
            _settingService = settingService;
            _mapper = mapper;
        }
        [HttpGet("GetAll/Business/{BusinessId}")]
        public async Task<IActionResult> GetBusinessSettings(long BusinessId)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartBranchBusinessSettingDTO>>(await _settingService.GetBranchBusinessSettingByBusinessIdAsync(BusinessId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBusinessSettingByID(long Id)
        {
            IEnumerable<SparePartBranchBusinessSettingDTO> list = _mapper.Map<IEnumerable<SparePartBranchBusinessSettingDTO>>(await _settingService.GetBranchBusinessSettingByIdAsync(Id));
            return Ok(list.FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(SparePartBranchBusinessSettingDTO model)
        {
            return Ok(_mapper.Map<SparePartBranchBusinessSettingDTO>(await _settingService.AddBranchBusinessSettingAsync(_mapper.Map<SparePartBranchBusinessSetting>(model))));
        }
        [HttpPut]
        public async Task<IActionResult> PutAsync(SparePartBranchBusinessSettingDTO model)
        {
            return Ok(_mapper.Map<SparePartBranchBusinessSettingDTO>(await _settingService.UpdateBranchBusinessSettingAsync(_mapper.Map<SparePartBranchBusinessSetting>(model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartCareerDTO>(await _settingService.ArchiveBranchBusinessSettingAsync(Id)));
        }

    }
}
