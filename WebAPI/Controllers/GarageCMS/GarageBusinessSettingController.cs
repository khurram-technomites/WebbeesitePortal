using AutoMapper;
using HelperClasses.DTOs;
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
    [Route("api/Garage/BusinessSetting")]
    [ApiController]
    public class GarageBusinessSettingController : ControllerBase
    {

        private readonly IGarageBusinessSettingService _settingService;
        private readonly IMapper _mapper;
        public GarageBusinessSettingController(IGarageBusinessSettingService settingService, IMapper mapper)
        {
            _settingService = settingService;
            _mapper = mapper;
        }
        [HttpGet("GetAll/{GarageId}")]
        public async Task<IActionResult> GetBusinessSettings(long GarageId)
        {
            return Ok(_mapper.Map<IEnumerable<GarageBusinessSettingDTO>>(await _settingService.GetBusinessSettingByGarageIdAsync(GarageId)));
        }
        [HttpGet("Get/{GarageId}")]
        public async Task<IActionResult> GetBusinessSettingByGarageId(long GarageId)
        {
            IEnumerable<GarageBusinessSettingDTO> list = _mapper.Map<IEnumerable<GarageBusinessSettingDTO>>(await _settingService.GetBusinessSettingByGarageId(GarageId));
            return Ok(list.FirstOrDefault());
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBusinessSettingByID(long Id)
        {
            IEnumerable<GarageBusinessSettingDTO> list = _mapper.Map<IEnumerable<GarageBusinessSettingDTO>>(await _settingService.GetBusinessSettingByIdAsync(Id));
            return Ok(list.FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(GarageBusinessSettingDTO model)
        {
            return Ok(_mapper.Map<GarageBusinessSettingDTO>(await _settingService.AddBusinessSettingAsync(_mapper.Map<GarageBusinessSetting>(model))));
        }
        [HttpPut]
        public async Task<IActionResult> PutAsync(GarageBusinessSettingDTO model)
        {
            return Ok(_mapper.Map<GarageBusinessSettingDTO>(await _settingService.UpdateBusinessSettingAsync(_mapper.Map<GarageBusinessSetting>(model))));
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
