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
    public class GarageTeamManagementController : ControllerBase
    {
        private readonly IGarageTeamManagementService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IGarageService _garageService;
        private readonly UserManager<AppUser> _userManager;
        public GarageTeamManagementController(IGarageTeamManagementService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, IFTPUpload fTPUpload, IGarageService garageService)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _garageService = garageService;
            _fTPUpload = fTPUpload;

        }

        [HttpGet("TeamManagement")]
        public async Task<IActionResult> GetAllGarageTeamManagement()
        {
            return Ok(_mapper.Map<IEnumerable<GarageTeamManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("TeamManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageTeamManagementDTO>>(await _service.GetGarageTeamManagementByIdAsync(Id)));
        }

        [HttpGet("{Id}/TeamManagement")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageTeamManagementDTO>>(await _service.GetGarageTeamManagementByGarageIdAsync(Id)));
        }
        [HttpGet("Count/{Id}/TeamManagement")]
        public async Task<IActionResult> GetCountAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<long>(await _service.GetGarageTeamManagementCountByGarageIdAsync(Id)));
        }

        [HttpPost("TeamManagement")]
        public async Task<IActionResult> Add(GarageTeamManagementDTO Model)
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

            return Ok(_mapper.Map<GarageTeamManagementDTO>(await _service.AddGarageTeamManagementAsync(_mapper.Map<GarageTeamManagement>(Model))));
        }

        [HttpPut("TeamManagement")]
        public async Task<IActionResult> Update(GarageTeamManagementDTO Model)
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

            return Ok(_mapper.Map<GarageTeamManagementDTO>(await _service.UpdateGarageTeamManagementAsync(_mapper.Map<GarageTeamManagement>(Model))));
        }

        [HttpDelete("TeamManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageTeamManagementDTO>(await _service.ArchiveGarageTeamManagementAsync(Id)));
        }

    }
}
