using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Interfaces.IServices;
using WebAPI.Models;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelperClasses.DTOs.SparePartsDealer;
using System.Linq;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartPartnersManagementController : ControllerBase
    {
        private readonly ISparePartPartnersManagementService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;
        public SparePartPartnersManagementController(ISparePartPartnersManagementService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("PartnersManagement")]
        public async Task<IActionResult> GetAllGaragePartnersManagement()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartPartnersManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("PartnersManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartPartnersManagementDTO>>(await _service.GetSparePartPartnersManagementByIdAsync(Id)));
        }

        [HttpGet("{Id}/Position")]
        public async Task<IActionResult> MaxPosition(long Id)
        {
            return Ok(await _service.GetPositionCount(Id));
        }

        [HttpGet("{Id}/PartnersManagement")]
        public async Task<IActionResult> GetAllBySparePartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartPartnersManagementDTO>>(await _service.GetSparePartPartnersManagementBySparePartDealerIdAsync(Id)));
        }

        [HttpPost("PartnersManagement")]
        public async Task<IActionResult> Add(SparePartPartnersManagementDTO Model)
        {
            long count = await _service.GetPositionCount(Model.SparePartDealerId);

            Model.Position = (int)(count + 1);

            IEnumerable<SparePartsDealerDTO> SparePartsDealer = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId));

            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }

            return Ok(_mapper.Map<SparePartPartnersManagementDTO>(await _service.AddSparePartPartnersManagementAsync(_mapper.Map<SparePartPartnersManagement>(Model))));

        }

        [HttpPut("PartnersManagement")]
        public async Task<IActionResult> Update(SparePartPartnersManagementDTO Model)
        {
            IEnumerable<SparePartsDealerDTO> SparePartsDealer = _mapper.Map<IEnumerable<SparePartsDealerDTO>>(await _sparePartsService.GetSparePartsDealerByIdAsync(Model.SparePartDealerId));

            if (!string.IsNullOrEmpty(Model.ImagePath) && Model.ImagePath.Contains("Draft"))
            {
                string LogoPath = "/Images/SparePart/" + SparePartsDealer.FirstOrDefault().NameAsPerTradeLicense + "/";
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }

            return Ok(_mapper.Map<SparePartPartnersManagementDTO>(await _service.UpdateSparePartPartnersManagementAsync(_mapper.Map<SparePartPartnersManagement>(Model))));
        }

        [HttpDelete("PartnersManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartPartnersManagementDTO>(await _service.ArchiveSparePartPartnersManagementAsync(Id)));
        }

        [HttpPost("PartnersManagement/SavePositions")]
        public async Task<IActionResult> SavePosition(SparePartPartnersManagementDTO Model)
        {
            IEnumerable<SparePartPartnersManagement> item = await _service.GetSparePartPartnersManagementByIdAsync(Model.Id);
            SparePartPartnersManagement menu = item.FirstOrDefault();
            menu.Position = Model.Position;

            return Ok(await _service.UpdateSparePartPartnersManagementAsync(menu));
        }
    }
}
