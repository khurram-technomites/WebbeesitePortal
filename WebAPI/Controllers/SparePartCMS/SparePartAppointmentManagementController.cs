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
using System.Linq;
using HelperClasses.DTOs.SparePartsDealer;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartAppointmentManagementController : ControllerBase
    {
        private readonly ISparePartAppointmentManagementService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;

        public SparePartAppointmentManagementController(ISparePartAppointmentManagementService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("AppointmentManagement")]
        public async Task<IActionResult> GetAllGarageAppointmentManagement()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartAppointmentManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("AppointmentManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartAppointmentManagementDTO>>(await _service.GetSparePartAppointmentManagementByIdAsync(Id)));
        }

        [HttpGet("{Id}/AppointmentManagement")]
        public async Task<IActionResult> GetAllBySparePartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartAppointmentManagementDTO>>(await _service.GetSparePartAppointmentManagementBySparePartDealerIdAsync(Id)));
        }

        [HttpPost("AppointmentManagement")]
        public async Task<IActionResult> Add(SparePartAppointmentManagementDTO Model)
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

            return Ok(_mapper.Map<SparePartAppointmentManagementDTO>(await _service.AddSparePartAppointmentManagementAsync(_mapper.Map<SparePartAppointmentManagement>(Model))));
        }

        [HttpPut("AppointmentManagement")]
        public async Task<IActionResult> Update(SparePartAppointmentManagementDTO Model)
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

            return Ok(_mapper.Map<SparePartAppointmentManagementDTO>(await _service.UpdateSparePartAppointmentManagementAsync(_mapper.Map<SparePartAppointmentManagement>(Model))));
        }

        [HttpDelete("AppointmentManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartAppointmentManagementDTO>(await _service.ArchiveSparePartAppointmentManagementAsync(Id)));
        }
    }
}
