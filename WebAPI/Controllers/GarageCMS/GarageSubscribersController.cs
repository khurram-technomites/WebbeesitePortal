using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    public class GarageSubscribersController : ControllerBase
    {
        private readonly IGarageSubscribersService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmailService _emailService;
        public GarageSubscribersController(IGarageSubscribersService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, IEmailService emailService)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpGet("Subscribers")]
        public async Task<IActionResult> GetAllGarageSubscribers()
        {
            return Ok(_mapper.Map<IEnumerable<GarageSubscribersDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("Subscribers/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageSubscribersDTO>>(await _service.GetGarageSubscribersByIdAsync(Id)));
        }

        [HttpGet("{Id}/Subscribers")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageSubscribersDTO>>(await _service.GetGarageSubscribersByGarageIdAsync(Id)));
        }

        [HttpPost("Subscribers")]
        public async Task<IActionResult> Add(GarageSubscribersDTO Model)
        {

            return Ok(_mapper.Map<GarageSubscribersDTO>(await _service.AddGarageSubscribersAsync(_mapper.Map<GarageSubscribers>(Model))));
        }

        [HttpPut("Subscribers")]
        public async Task<IActionResult> Update(GarageSubscribersDTO Model)
        {
            return Ok(_mapper.Map<GarageSubscribersDTO>(await _service.UpdateGarageSubscribersAsync(_mapper.Map<GarageSubscribers>(Model))));
        }

        [HttpDelete("Subscribers/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            await _service.ArchiveGarageSubscribersAsync(Id);
            return Ok();
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendEmail(GarageSubscribersDTO Model)
        {
            IEnumerable<GarageSubscribers> custList = await _service.GetGarageSubscribersByIdAsync(Model.Id);

            string email = custList.FirstOrDefault().Email;

            await _emailService.SendSubscriberEmail(Model.Email, email, Model.Message);

            return Ok(new SuccessResponse<string>("Email sent successfully", null));
        }
    }
}
