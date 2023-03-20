using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Supplier;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "GarageOwner")]
    public class GarageBannerSettingController : ControllerBase
    {
        private readonly IGarageBannerSettingService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFTPUpload _fTPUpload;
        private readonly IGarageService _garageService;
        public GarageBannerSettingController(IGarageBannerSettingService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, IFTPUpload fTPUpload, IGarageService garageService)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _fTPUpload = fTPUpload;
            _garageService = garageService;
        }

        [HttpGet("BannerSetting")]
        public async Task<IActionResult> GetAllGarageBannerSetting()
        {
            return Ok(_mapper.Map<IEnumerable<GarageBannerSettingDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("BannerSetting/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            IEnumerable<GarageBannerSettingDTO> list = _mapper.Map<IEnumerable<GarageBannerSettingDTO>>(await _service.GetGarageBannerSettingByIdAsync(Id));
            return Ok(list.FirstOrDefault());
        }

        [HttpGet("{Id}/BannerSetting")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageBannerSettingDTO>>(await _service.GetGarageBannerSettingByGaragedIdAsync(Id)));
        }

        [HttpPost("BannerSetting")]
        public async Task<IActionResult> Add(GarageBannerSettingDTO Model)
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
            if (!string.IsNullOrEmpty(Model.Thumbnail))
            {
                string LogoPath = "/Images/Garage/" + Garage.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.Thumbnail, ref LogoPath))
                {
                    Model.Thumbnail = LogoPath;
                }
            }

            return Ok(_mapper.Map<GarageBannerSettingDTO>(await _service.AddGarageBannerSettingAsync(_mapper.Map<GarageBannerSetting>(Model))));
        }

        [HttpPut("BannerSetting")]
        public async Task<IActionResult> Update(GarageBannerSettingDTO Model)
        {
            IEnumerable<Garage> Garage = await _garageService.GetGarageByIdAsync(Model.Id);

            if (Model.ImagePath != null && !Model.ImagePath.Replace("%20", " ").Equals(Model.ImagePath))
            {
                string LogoPath = "/Images/Garage/" + Garage.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }
            if (Model.Thumbnail != null && !Model.Thumbnail.Replace("%20", " ").Equals(Model.Thumbnail))
            {
                string LogoPath = "/Images/Garage/" + Garage.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.Thumbnail, ref LogoPath))
                {
                    Model.Thumbnail = LogoPath;
                }
            }

            return Ok(_mapper.Map<GarageBannerSettingDTO>(await _service.UpdateGarageBannerSettingAsync(_mapper.Map<GarageBannerSetting>(Model))));
        }

        [HttpDelete("BannerSetting/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageBannerSettingDTO>(await _service.ArchiveGarageBannerSettingAsync(Id)));
        }

    }
}
