using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Interfaces.IServices;
using WebAPI.Models;
using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    public class GarageCustomerFeedbackController : ControllerBase
    {
        private readonly IGarageCustomerFeedbackService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IGarageService _garageService;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;
        public GarageCustomerFeedbackController(IGarageCustomerFeedbackService service, IMapper mapper, IUserService userService, IGarageService garageService, IFTPUpload fTPUpload, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _garageService = garageService;
            _fTPUpload = fTPUpload;
            _userManager = userManager;
        }

        [HttpGet("CustomerFeedback")]
        public async Task<IActionResult> GetAllCustomerFeedback()
        {
            return Ok(_mapper.Map<IEnumerable<GarageCustomerFeedbackDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("CustomerFeedback/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageCustomerFeedbackDTO>>(await _service.GetGarageCareersByIdAsync(Id)));
        }

        [HttpGet("{Id}/CustomerFeedback")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageCustomerFeedbackDTO>>(await _service.GetGarageCustomerFeedbackByGarageIdAsync(Id)));
        }
        [HttpPost("CustomerFeedback")]
        public async Task<IActionResult> Add(GarageCustomerFeedbackDTO Model)
        {

            return Ok(_mapper.Map<GarageCustomerFeedbackDTO>(await _service.AddGarageCustomerFeedbackAsync(_mapper.Map<GarageCustomerFeedback>(Model))));
        }

        [HttpPut("CustomerFeedback")]
        public async Task<IActionResult> Update(GarageCustomerFeedbackDTO Model)
        {
            return Ok(_mapper.Map<GarageCustomerFeedbackDTO>(await _service.UpdateGarageCustomerFeedbackAsync(_mapper.Map<GarageCustomerFeedback>(Model))));
        }
        [HttpDelete("CustomerFeedback/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageCustomerFeedbackDTO>(await _service.ArchiveGarageCustomerFeedbackAsync(Id)));
        }
    }
}
