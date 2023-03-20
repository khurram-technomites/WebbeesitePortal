using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    public class GarageServiceManagementController : ControllerBase
    {
        private readonly IGarageServiceManagementService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IGarageService _garageService;
        private readonly INumberRangeService _numberRangeService;
        private readonly UserManager<AppUser> _userManager;

        public GarageServiceManagementController(IGarageServiceManagementService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, 
            IFTPUpload fTPUpload, IGarageService garageService, INumberRangeService numberRangeService)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _fTPUpload = fTPUpload;
            _garageService = garageService;
            _numberRangeService = numberRangeService;
        }

        [HttpGet("ServiceManagement")]
        public async Task<IActionResult> GetAllGarageServiceManagement()
        {
            return Ok(_mapper.Map<IEnumerable<GarageServiceManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("ServiceManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageServiceManagementDTO>>(await _service.GetGarageServiceManagementByIdAsync(id)));
        }

        [HttpGet("{Id}/ServiceManagement")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageServiceManagementDTO>>(await _service.GetGarageServiceManagementByGaragedIdAsync(Id)));
        }
        [HttpGet("Count/{Id}/ServiceManagement")]
        public async Task<IActionResult> GetCountAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<long>(await _service.GetGarageServiceManagementCountByGaragedIdAsync(Id)));
        }
        [HttpPost("ServiceManagement")]
        public async Task<IActionResult> Add(GarageServiceManagementDTO Model)
        {
            IEnumerable<Garage> Garages = await _garageService.GetGarageByIdAsync(Model.GarageId);

            if (!string.IsNullOrEmpty(Model.Icon))
            {
                string LogoPath = "/Images/Garage/" + Garages.FirstOrDefault().Id + "/";
                if (_fTPUpload.MoveFile(Model.Icon, ref LogoPath))
                {
                    Model.Icon = LogoPath;
                }
            }

            if (!string.IsNullOrEmpty(Model.BannerImagePath))
            {
                string LogoPath = "/Images/Garage/" + Garages.FirstOrDefault().Id + "/";
                if (_fTPUpload.MoveFile(Model.BannerImagePath, ref LogoPath))
                {
                    Model.BannerImagePath = LogoPath;
                }
            }
            if (!string.IsNullOrEmpty(Model.Thumbnail))
            {
                string LogoPath = "/Images/Garage/" + Garages.FirstOrDefault().Id + "/";
                if (_fTPUpload.MoveFile(Model.Thumbnail, ref LogoPath))
                {
                    Model.Thumbnail = LogoPath;
                }
            }

            Model.Slug = Slugify.GenerateSlug(Model.Title, await _numberRangeService.GetNumberRangeByName("GARAGESERVICE"));
            return Ok(_mapper.Map<GarageServiceManagementDTO>(await _service.AddGarageServiceManagementAsync(_mapper.Map<GarageServiceManagement>(Model))));
        }

        [HttpPut("ServiceManagement")]
        public async Task<IActionResult> Update(GarageServiceManagementDTO Model)
        {
            IEnumerable<Garage> Garage = await _garageService.GetGarageByIdAsync(Model.GarageId);

            if (!string.IsNullOrEmpty(Model.Icon) && Model.Icon.Contains("Draft"))
            {
                string LogoPath = "/Images/Garage/" + Garage.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.Icon, ref LogoPath))
                {
                    Model.Icon = LogoPath;
                }
            }

            if (!string.IsNullOrEmpty(Model.BannerImagePath) && Model.BannerImagePath.Contains("Draft"))
            {
                string LogoPath = "/Images/Garage/" + Garage.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.BannerImagePath, ref LogoPath))
                {
                    Model.BannerImagePath = LogoPath;
                }
            }
            if (!string.IsNullOrEmpty(Model.Thumbnail) && Model.Thumbnail.Contains("Draft"))
            {
                string LogoPath = "/Images/Garage/" + Garage.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.Thumbnail, ref LogoPath))
                {
                    Model.Thumbnail = LogoPath;
                }
            }

            if (string.IsNullOrEmpty(Model.Slug))
                Model.Slug = Slugify.GenerateSlug(Model.Title, await _numberRangeService.GetNumberRangeByName("GARAGESERVICE"));

            return Ok(_mapper.Map<GarageServiceManagementDTO>(await _service.UpdateGarageServiceManagementAsync(_mapper.Map<GarageServiceManagement>(Model))));
        }

        [HttpDelete("ServiceManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageServiceManagementDTO>(await _service.ArchiveGarageServiceManagementAsync(Id)));
        }

        [HttpGet("ServiceManagement/ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {            
            IEnumerable<GarageServiceManagement> list = await _service.GetGarageServiceManagementByIdAsync(Id);
            GarageServiceManagement content = list.FirstOrDefault();

            if (content.Status == Enum.GetName(typeof(Status), Status.Active))
                content.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                content.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<GarageServiceManagementDTO>(await _service.UpdateGarageServiceManagementAsync(content)));

        }
    }
}
