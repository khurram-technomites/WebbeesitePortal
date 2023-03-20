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
using WebAPI.Services.Domains;
using HelperClasses.DTOs.SparePartsDealer;
using HelperClasses.DTOs.SparePartCMS;
using System.Linq;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartBannerSettingController : ControllerBase
    {
        private readonly ISparePartBannerSettingService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFTPUpload _fTPUpload;
        private readonly ISparePartsDealerService _sparePartsService;

        public SparePartBannerSettingController(ISparePartBannerSettingService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, IFTPUpload fTPUpload, ISparePartsDealerService sparePartsService)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _fTPUpload = fTPUpload;
            _sparePartsService = sparePartsService;
        }

        [HttpGet("BannerSetting")]
        public async Task<IActionResult> GetAllGarageBannerSetting()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartBannerSettingDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("BannerSetting/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            IEnumerable<SparePartBannerSettingDTO> list = _mapper.Map<IEnumerable<SparePartBannerSettingDTO>>(await _service.GetSparePartBannerSettingByIdAsync(Id));
            return Ok(list.FirstOrDefault());
        }

        [HttpGet("{Id}/BannerSetting")]
        public async Task<IActionResult> GetAllBySparePartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartBannerSettingDTO>>(await _service.GetSparePartBannerSettingBySparePartDealerIdAsync(Id)));
        }

        [HttpPost("BannerSetting")]
        public async Task<IActionResult> Add(SparePartBannerSettingDTO Model)
        {
            IEnumerable<SparePartsDealerDTO> SparePartsDealer = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId));

            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }

            return Ok(_mapper.Map<SparePartBannerSettingDTO>(await _service.AddSparePartBannerSettingAsync(_mapper.Map<SparePartBannerSetting>(Model))));
        }

        [HttpPut("BannerSetting")]
        public async Task<IActionResult> Update(SparePartBannerSettingDTO Model)
        {
            IEnumerable<SparePartsDealerDTO> SparePartsDealer = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId));

            if (Model.ImagePath != null && !Model.ImagePath.Replace("%20", " ").Equals(Model.ImagePath))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }

            return Ok(_mapper.Map<SparePartBannerSettingDTO>(await _service.UpdateSparePartBannerSettingAsync(_mapper.Map<SparePartBannerSetting>(Model))));
        }

        [HttpDelete("BannerSetting/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartBannerSettingDTO>(await _service.ArchiveSparePartBannerSettingAsync(Id)));
        }
    }
}
