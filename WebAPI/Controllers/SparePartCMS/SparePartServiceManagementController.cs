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
using System;
using WebAPI.Helpers;
using WebAPI.Services.Domains;
using HelperClasses.DTOs.SparePartCMS;
using HelperClasses.DTOs.SparePartsDealer;
using System.Linq;
using HelperClasses.Classes;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartServiceManagementController : ControllerBase
    {
        private readonly ISparePartServiceManagement _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;
        private readonly INumberRangeService _numberRangeService;

        public SparePartServiceManagementController(ISparePartServiceManagement service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload, INumberRangeService numberRangeService)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
            _numberRangeService = numberRangeService;
        }

        [HttpGet("ServiceManagement")]
        public async Task<IActionResult> GetAllGarageServiceManagement()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartServiceManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("ServiceManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartServiceManagementDTO>>(await _service.GetSparePartServiceManagementByIdAsync(id)));
        }

        [HttpGet("{Id}/ServiceManagement")]
        public async Task<IActionResult> GetAllBySparePartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartServiceManagementDTO>>(await _service.GetSparePartServiceManagementBySparePartDealerIdAsync(Id)));
        }

        [HttpPost("ServiceManagement")]
        public async Task<IActionResult> Add(SparePartServiceManagementDTO Model)
        {
            IEnumerable<SparePartsDealerDTO> SparePartsDealer = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId));

            if (!string.IsNullOrEmpty(Model.Icon))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.Icon, ref LogoPath))
                {
                    Model.Icon = LogoPath;
                }
            }

            if (!string.IsNullOrEmpty(Model.BannerImagePath))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.BannerImagePath, ref LogoPath))
                {
                    Model.BannerImagePath = LogoPath;
                }
            }

            Model.Slug = Slugify.GenerateSlug(Model.Title, await _numberRangeService.GetNumberRangeByName("SPAREPARTDEALERSERVICE"));
            return Ok(_mapper.Map<SparePartServiceManagementDTO>(await _service.AddSparePartServiceManagementAsync(_mapper.Map<SparePartServiceManagement>(Model))));
        }

        [HttpPut("ServiceManagement")]
        public async Task<IActionResult> Update(SparePartServiceManagementDTO Model)
        {
            IEnumerable<SparePartsDealerDTO> SparePartsDealer = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId));

            if (!string.IsNullOrEmpty(Model.Icon) && Model.Icon.Contains("Draft"))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.Icon, ref LogoPath))
                {
                    Model.Icon = LogoPath;
                }
            }

            if (!string.IsNullOrEmpty(Model.BannerImagePath) && Model.BannerImagePath.Contains("Draft"))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.BannerImagePath, ref LogoPath))
                {
                    Model.BannerImagePath = LogoPath;
                }
            }

            if (string.IsNullOrEmpty(Model.Slug))
                Model.Slug = Slugify.GenerateSlug(Model.Title, await _numberRangeService.GetNumberRangeByName("SPAREPARTDEALERSERVICE"));

            return Ok(_mapper.Map<SparePartServiceManagementDTO>(await _service.UpdateSparePartServiceManagementAsync(_mapper.Map<SparePartServiceManagement>(Model))));
        }

        [HttpDelete("ServiceManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartServiceManagementDTO>(await _service.ArchiveSparePartServiceManagementAsync(Id)));
        }

        [HttpGet("ServiceManagement/ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<SparePartServiceManagement> list = await _service.GetSparePartServiceManagementByIdAsync(Id);
            SparePartServiceManagement content = list.FirstOrDefault();

            if (content.Status == Enum.GetName(typeof(Status), Status.Active))
                content.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                content.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<SparePartServiceManagementDTO>(await _service.UpdateSparePartServiceManagementAsync(content)));

        }
    }
}
