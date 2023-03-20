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
using HelperClasses.DTOs.SparePartCMS;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartCustomerFeedbackController : ControllerBase
    {
        private readonly ISparePartCustomerFeedbackService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;
        public SparePartCustomerFeedbackController(ISparePartCustomerFeedbackService service, IMapper mapper, IUserService userService, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
            _userManager = userManager;
        }

        [HttpGet("CustomerFeedback")]
        public async Task<IActionResult> GetAllCustomerFeedback()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartCustomerFeedbackDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("CustomerFeedback/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartCustomerFeedbackDTO>>(await _service.GetSparePartCustomerByIdAsync(Id)));
        }

        [HttpGet("{Id}/CustomerFeedback")]
        public async Task<IActionResult> GetAllBySparePartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartCustomerFeedbackDTO>>(await _service.GetSparePartCustomerFeedbackBySparePartDealerIdAsync(Id)));
        }
        [HttpPost("CustomerFeedback")]
        public async Task<IActionResult> Add(SparePartCustomerFeedbackDTO Model)
        {

            return Ok(_mapper.Map<SparePartCustomerFeedbackDTO>(await _service.AddSparePartCustomerFeedbackAsync(_mapper.Map<SparePartCustomerFeedback>(Model))));
        }

        [HttpPut("CustomerFeedback")]
        public async Task<IActionResult> Update(SparePartCustomerFeedbackDTO Model)
        {
            return Ok(_mapper.Map<SparePartCustomerFeedbackDTO>(await _service.UpdateSparePartCustomerFeedbackAsync(_mapper.Map<SparePartCustomerFeedback>(Model))));
        }
        [HttpDelete("CustomerFeedback/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartCustomerFeedbackDTO>(await _service.ArchiveSparePartCustomerFeedbackAsync(Id)));
        }
    }
}
