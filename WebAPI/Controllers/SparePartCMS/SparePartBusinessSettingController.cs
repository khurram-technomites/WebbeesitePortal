using AutoMapper;
using HelperClasses.DTOs;
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
    [Route("api/SparePart/BusinessSetting")]
    [ApiController]
    public class SparePartBusinessSettingController : ControllerBase
    {

        private readonly ISparePartBusinessSettingService _settingService;
        private readonly IMapper _mapper;
        public SparePartBusinessSettingController(ISparePartBusinessSettingService settingService, IMapper mapper)
        {
            _settingService = settingService;
            _mapper = mapper;
        }
        [HttpGet("GetAll/{SparePartId}")]
        public async Task<IActionResult> GetBusinessSettings(long SparePartId)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartBusinessSettingDTO>>(await _settingService.GetBusinessSettingBySparePartIdAsync(SparePartId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBusinessSettingByID(long Id)
        {
            IEnumerable<SparePartBusinessSettingDTO> list = _mapper.Map<IEnumerable<SparePartBusinessSettingDTO>>(await _settingService.GetBusinessSettingByIdAsync(Id));
            return Ok(list.FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(SparePartBusinessSettingDTO model)
        {
            return Ok(_mapper.Map<SparePartBusinessSettingDTO>(await _settingService.AddBusinessSettingAsync(_mapper.Map<SparePartBusinessSetting>(model))));
        }
        [HttpPut]
        public async Task<IActionResult> PutAsync(SparePartBusinessSettingDTO model)
        {
            return Ok(_mapper.Map<SparePartBusinessSettingDTO>(await _settingService.UpdateBusinessSettingAsync(_mapper.Map<SparePartBusinessSetting>(model))));
        }

        //[HttpGet("Policy/{PolicyName}")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetPolicy(string PolicyName)
        //{
        //    IEnumerable<BusinessSettings> Policies = await _settingService.GetAllBusinessSettingAsync();

        //    if (PolicyName.ToLower().Replace(" ", "") == "privacypolicy")
        //        return Ok(new SuccessResponse<string>("", Policies.FirstOrDefault().PrivacyPolicy));

        //    if (PolicyName.ToLower().Replace(" ", "") == "deliverypolicy")
        //        return Ok(new SuccessResponse<string>("", Policies.FirstOrDefault().DeliveryPolicy));

        //    if (PolicyName.ToLower().Replace(" ", "") == "returnpolicy")
        //        return Ok(new SuccessResponse<string>("", Policies.FirstOrDefault().ReturnPolicy));

        //    if (PolicyName.ToLower().Replace(" ", "") == "termsandcondition")
        //        return Ok(new SuccessResponse<string>("", Policies.FirstOrDefault().TermsAndConditions));

        //    return Conflict(new ErrorDetails(409, "Invalid Policy", null));

        //}
    }
}
