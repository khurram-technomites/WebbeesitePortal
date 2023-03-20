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

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    public class GaragePartnersManagementController : ControllerBase
    {
        private readonly IGaragePartnersManagementService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IGarageService _garageService;
        private readonly IFTPUpload _fTPUpload;
        public GaragePartnersManagementController(IGaragePartnersManagementService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, IFTPUpload fTPUpload, IGarageService garageService)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _garageService = garageService;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("PartnersManagement")]
        public async Task<IActionResult> GetAllGaragePartnersManagement()
        {
            return Ok(_mapper.Map<IEnumerable<GaragePartnersManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("PartnersManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GaragePartnersManagementDTO>>(await _service.GetGaragePartnersManagementByIdAsync(Id)));
        }

        [HttpGet("{Id}/Position")]
        public async Task<IActionResult> MaxPosition(long Id)
        {
            return Ok(await _service.GetPositionCount(Id));
        }

        [HttpGet("{Id}/PartnersManagement")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GaragePartnersManagementDTO>>(await _service.GetGaragePartnersManagementByGarageIdAsync(Id)));
        }
        [HttpGet("Count/{Id}/PartnersManagement")]
        public async Task<IActionResult> GetAllCountByGarageId(long Id)
        {
            return Ok(_mapper.Map<long>(await _service.GetGaragePartnersManagementCountByGarageIdAsync(Id)));
        }
        [HttpPost("PartnersManagement")]
        public async Task<IActionResult> Add(GaragePartnersManagementDTO Model)
        {
            long count = await _service.GetPositionCount(Model.GarageId);

            Model.Position = (int)(count + 1);

            IEnumerable<Garage> Garage = await _garageService.GetGarageByIdAsync(Model.GarageId);

            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                string LogoPath = "/Images/Garage/" + Garage.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }

            return Ok(_mapper.Map<GaragePartnersManagementDTO>(await _service.AddGaragePartnersManagementAsync(_mapper.Map<GaragePartnersManagement>(Model))));

        }

        [HttpPut("PartnersManagement")]
        public async Task<IActionResult> Update(GaragePartnersManagementDTO Model)
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

            return Ok(_mapper.Map<GaragePartnersManagementDTO>(await _service.UpdateGaragePartnersManagementAsync(_mapper.Map<GaragePartnersManagement>(Model))));
        }

        [HttpDelete("PartnersManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GaragePartnersManagementDTO>(await _service.ArchiveGaragePartnersManagementAsync(Id)));
        }

        [HttpPost("PartnersManagement/SavePositions")]
        public async Task<IActionResult> SavePosition(GaragePartnersManagementDTO Model)
        {
            IEnumerable<GaragePartnersManagement> item = await _service.GetGaragePartnersManagementByIdAsync(Model.Id);
            GaragePartnersManagement menu = item.FirstOrDefault();
            menu.Position = Model.Position;

            return Ok(await _service.UpdateGaragePartnersManagementAsync(menu));
        }
    }
}
