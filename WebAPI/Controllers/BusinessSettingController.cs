using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ErrorHandling;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class BusinessSettingController : ControllerBase
    {
        private readonly IBusinessSettingService _settingService;
        private readonly IMapper _mapper;
        public BusinessSettingController(IBusinessSettingService settingService, IMapper mapper)
        {
            _settingService = settingService;
            _mapper = mapper;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBusinessSettings()
        {
            return Ok(_mapper.Map<IEnumerable<BusinessSettingDTO>>(await _settingService.GetAllBusinessSettingAsync()));
        }

        [HttpGet("Master")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBusinessSettingsMaster()
        {
            return Ok(new SuccessResponse<IEnumerable<BusinessSettingDTO>>("", _mapper.Map<IEnumerable<BusinessSettingDTO>>(await _settingService.GetAllBusinessSettingAsync())));
        }
        
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBusinessSettingByID(long Id)
        {
            IEnumerable<BusinessSettingDTO> list = _mapper.Map<IEnumerable<BusinessSettingDTO>>(await _settingService.GetBusinessSettingByIdAsync(Id));
            return Ok(list.FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(BusinessSettingDTO model)
        {
            return Ok(_mapper.Map<BusinessSettingDTO>(await _settingService.AddBusinessSettingAsync(_mapper.Map<BusinessSettings>(model))));
        }
        [HttpPut]
        public async Task<IActionResult> PutAsync(BusinessSettingDTO model)
        {
            return Ok(_mapper.Map<BusinessSettingDTO>(await _settingService.UpdateBusinessSettingAsync(_mapper.Map<BusinessSettings>(model))));
        }

        [HttpGet("Policy/{PolicyName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPolicy(string PolicyName)
        {
            IEnumerable<BusinessSettings> Policies = await _settingService.GetAllBusinessSettingAsync();

            if(PolicyName.ToLower().Replace(" ", "") == "privacypolicy")
                return Ok(new SuccessResponse<string>("", Policies.FirstOrDefault().PrivacyPolicy));

            if (PolicyName.ToLower().Replace(" ", "") == "deliverypolicy")
                return Ok(new SuccessResponse<string>("", Policies.FirstOrDefault().DeliveryPolicy));

            if (PolicyName.ToLower().Replace(" ", "") == "returnpolicy")
                return Ok(new SuccessResponse<string>("", Policies.FirstOrDefault().ReturnPolicy));

            if (PolicyName.ToLower().Replace(" ", "") == "termsandcondition")
                return Ok(new SuccessResponse<string>("", Policies.FirstOrDefault().TermsAndConditions));

            return Conflict(new ErrorDetails(409, "Invalid Policy", null));

        }

    }
}
