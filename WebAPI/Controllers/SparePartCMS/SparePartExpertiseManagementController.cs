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

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartExpertiseManagementController : ControllerBase
    {
        private readonly ISparePartExpertiseManagementService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;
        public SparePartExpertiseManagementController(ISparePartExpertiseManagementService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("ExpertiseManagement")]
        public async Task<IActionResult> GetAllGarageExpertiseManagementManagement()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartExpertiseManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("ExpertiseManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartExpertiseManagementDTO>>(await _service.GetSparePartExpertiseManagementByIdAsync(Id)));
        }

        [HttpGet("{Id}/ExpertiseManagement")]
        public async Task<IActionResult> GetAllBySparePartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartExpertiseManagementDTO>>(await _service.GetSparePartExpertiseManagementBySparePartDealerIdAsync(Id)));
        }

        [HttpPost("ExpertiseManagement")]
        public async Task<IActionResult> Add(SparePartExpertiseManagementDTO Model)
        {
            string LogoPath = "/Images/SparePart/ExpertiseManagement/";
            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }
            return Ok(_mapper.Map<SparePartExpertiseManagementDTO>(await _service.AddSparePartExpertiseManagementAsync(_mapper.Map<SparePartExpertiseManagement>(Model))));
        }

        [HttpPut("ExpertiseManagement")]
        public async Task<IActionResult> Update(SparePartExpertiseManagementDTO Model)
        {
            string LogoPath = "/Images/SparePart/ExpertiseManagement/";
            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }
            return Ok(_mapper.Map<SparePartExpertiseManagementDTO>(await _service.UpdateSparePartExpertiseManagementAsync(_mapper.Map<SparePartExpertiseManagement>(Model))));
        }

        [HttpDelete("ExpertiseManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartExpertiseManagementDTO>(await _service.ArchiveSparePartExpertiseManagementAsync(Id)));
        }

        [HttpDelete("ExpertiseManagement/Delete/{Id}")]
        public IActionResult Delete(long Id)
        {
            return Ok(_service.DeleteSparePartExpertiseManagementAsync(Id));
        }
    }
}
