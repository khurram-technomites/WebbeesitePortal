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
using WebAPI.Services.Domains;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    public class GarageAppointmentManagementController : ControllerBase
    {
        private readonly IGarageAppointmentManagementService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGarageService _garageService;
        private readonly IFTPUpload _fTPUpload;
        public GarageAppointmentManagementController(IGarageAppointmentManagementService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, IFTPUpload fTPUpload, IGarageService garageService)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _garageService = garageService;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("AppointmentManagement")]
        public async Task<IActionResult> GetAllGarageAppointmentManagement()
        {
            return Ok(_mapper.Map<IEnumerable<GarageAppointmentManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("AppointmentManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageAppointmentManagementDTO>>(await _service.GetGarageGarageAppointmentManagementByIdAsync(Id)));
        }

        [HttpGet("{Id}/AppointmentManagement")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageAppointmentManagementDTO>>(await _service.GetGarageAppointmentManagementByGarageIdAsync(Id)));
        }

        [HttpPost("AppointmentManagement")]
        public async Task<IActionResult> Add(GarageAppointmentManagementDTO Model)
        {

            IEnumerable<Garage> Garage = await _garageService.GetGarageByIdAsync(Model.GarageId);

            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                string LogoPath = "/Images/Garage/" + Garage.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }

            return Ok(_mapper.Map<GarageAppointmentManagementDTO>(await _service.AddGarageAppointmentManagementAsync(_mapper.Map<GarageAppointmentManagement>(Model))));
        }

        [HttpPut("AppointmentManagement")]
        public async Task<IActionResult> Update(GarageAppointmentManagementDTO Model)
        {
            IEnumerable<Garage> Garage = await _garageService.GetGarageByIdAsync(Model.GarageId);

            if (!string.IsNullOrEmpty(Model.ImagePath) && Model.ImagePath.Contains("Draft"))
            {
                string LogoPath = "/Images/Garage/" + Garage.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }

            return Ok(_mapper.Map<GarageAppointmentManagementDTO>(await _service.UpdateGarageAppointmentManagementAsync(_mapper.Map<GarageAppointmentManagement>(Model))));
        }

        [HttpDelete("AppointmentManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageAppointmentManagementDTO>(await _service.ArchiveGarageAppointmentManagementAsync(Id)));
        }
    }
}
