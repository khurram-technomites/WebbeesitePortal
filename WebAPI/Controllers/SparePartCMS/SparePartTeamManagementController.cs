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
using HelperClasses.DTOs.SparePartCMS;
using HelperClasses.DTOs.SparePartsDealer;
using System.Linq;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartTeamManagementController : ControllerBase
    {
        private readonly ISparePartTeamManagementService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;
        public SparePartTeamManagementController(ISparePartTeamManagementService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("TeamManagement")]
        public async Task<IActionResult> GetAllGarageTeamManagement()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartTeamManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("TeamManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartTeamManagementDTO>>(await _service.GetSparePartTeamManagementByIdAsync(Id)));
        }

        [HttpGet("{Id}/TeamManagement")]
        public async Task<IActionResult> GetAllBySparePartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartTeamManagementDTO>>(await _service.GetSparePartTeamManagementBySparePartDealerIdAsync(Id)));
        }

        [HttpPost("TeamManagement")]
        public async Task<IActionResult> Add(SparePartTeamManagementDTO Model)
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

            return Ok(_mapper.Map<SparePartTeamManagementDTO>(await _service.AddSparePartTeamManagementtAsync(_mapper.Map<SparePartTeamManagement>(Model))));
        }

        [HttpPut("TeamManagement")]
        public async Task<IActionResult> Update(SparePartTeamManagementDTO Model)
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

            return Ok(_mapper.Map<SparePartTeamManagementDTO>(await _service.UpdateSparePartTeamManagementAsync(_mapper.Map<SparePartTeamManagement>(Model))));
        }

        [HttpDelete("TeamManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartTeamManagementDTO>(await _service.ArchiveSparePartTeamManagementAsync(Id)));
        }
    }
}
