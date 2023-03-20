using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    public class GarageCustomerAppointmentController : ControllerBase
    {
        private readonly IGarageCustomerAppointmentService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        public GarageCustomerAppointmentController(IGarageCustomerAppointmentService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet("CustomerAppointment")]
        public async Task<IActionResult> GetAllGarageContentManagement()
        {
            return Ok(_mapper.Map<IEnumerable<GarageCustomerAppointmentDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("CustomerAppointment/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageCustomerAppointmentDTO>>(await _service.GetGarageCustomerAppointmentByIdAsync(Id)));
        }

        [HttpGet("{Id}/CustomerAppointment")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageCustomerAppointmentDTO>>(await _service.GetGarageCustomerAppointmentByGarageIdAsync(Id)));
        }

        [HttpPost("CustomerAppointment")]
        public async Task<IActionResult> Add(GarageCustomerAppointmentDTO Model)
        {

            return Ok(_mapper.Map<GarageCustomerAppointmentDTO>(await _service.AddGarageCustomerAppointmentAsync(_mapper.Map<GarageCustomerAppointment>(Model))));
        }

        [HttpPut("CustomerAppointment")]
        public async Task<IActionResult> Update(GarageCustomerAppointmentDTO Model)
        {
            return Ok(_mapper.Map<GarageCustomerAppointmentDTO>(await _service.UpdateGarageCustomerAppointmentAsync(_mapper.Map<GarageCustomerAppointment>(Model))));
        }

        [HttpDelete("CustomerAppointment/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageCustomerAppointmentDTO>(await _service.ArchiveGarageCustomerAppointmentAsync(Id)));
        }
    }
}
