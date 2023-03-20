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
using WebAPI.ResponseWrapper;
using HelperClasses.DTOs.SparePartCMS;
using System.Linq;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartSubscriberController : ControllerBase
    {
        private readonly ISparePartSubscriberService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IEmailService _emailService;

        public SparePartSubscriberController(ISparePartSubscriberService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload, IEmailService emailService)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
            _emailService = emailService;
        }

        [HttpGet("Subscribers")]
        public async Task<IActionResult> GetAllGarageSubscribers()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartSubscriberDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("Subscribers/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartSubscriberDTO>>(await _service.GetSparePartSubscriberByIdAsync(Id)));
        }

        [HttpGet("{Id}/Subscribers")]
        public async Task<IActionResult> GetAllBySparePartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartSubscriberDTO>>(await _service.GetSparePartSubscriberBySparePartDealerIdAsync(Id)));
        }

        [HttpPost("Subscribers")]
        public async Task<IActionResult> Add(SparePartSubscriberDTO Model)
        {

            return Ok(_mapper.Map<SparePartSubscriberDTO>(await _service.AddSparePartSubscriberAsync(_mapper.Map<SparePartSubscriber>(Model))));
        }

        [HttpPut("Subscribers")]
        public async Task<IActionResult> Update(SparePartSubscriberDTO Model)
        {
            return Ok(_mapper.Map<SparePartSubscriberDTO>(await _service.UpdateSparePartSubscriberAsync(_mapper.Map<SparePartSubscriber>(Model))));
        }

        [HttpDelete("Subscribers/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            await _service.ArchiveSparePartSubscriberAsync(Id);
            return Ok();
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(GarageSubscribersDTO Model)
        {
            IEnumerable<SparePartSubscriber> custList = await _service.GetSparePartSubscriberByIdAsync(Model.Id);

            string email = custList.FirstOrDefault().Email;

            await _emailService.SendSubscriberEmail(Model.Email, email, Model.Message);

            return Ok(new SuccessResponse<string>("Email sent successfully", null));
        }
    }
}
