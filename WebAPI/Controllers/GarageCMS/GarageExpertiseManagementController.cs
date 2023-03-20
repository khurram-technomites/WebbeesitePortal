using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    public class GarageExpertiseManagementController : ControllerBase
    {
        private readonly IGarageExpertiseManagementService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFTPUpload _fTPUpload;
        public GarageExpertiseManagementController(IGarageExpertiseManagementService service, IFTPUpload fTPUpload, IMapper mapper, IUserService userService, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _fTPUpload = fTPUpload;
        }
        [HttpGet("ExpertiseManagement")]
        public async Task<IActionResult> GetAllGarageExpertiseManagementManagement()
        {
            return Ok(_mapper.Map<IEnumerable<GarageExpertiseManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("ExpertiseManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageExpertiseManagementDTO>>(await _service.GetGarageExpertiseManagementByIdAsync(Id)));
        }

        [HttpGet("{Id}/ExpertiseManagement")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageExpertiseManagementDTO>>(await _service.GetGarageExpertiseManagementByGarageIdAsync(Id)));
        }
        [HttpGet("Count/{Id}/ExpertiseManagement")]
        public async Task<IActionResult> GetCountAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<long>(await _service.GetGarageExpertiseCountByGarageIdAsnyc(Id)));
        }

        [HttpPost("ExpertiseManagement")]
        public async Task<IActionResult> Add(GarageExpertiseManagementDTO Model)
        {
            string LogoPath = "/Images/Garage/ExpertiseManagement/";
            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }
            return Ok(_mapper.Map<GarageExpertiseManagementDTO>(await _service.AddGarageExpertiseManagementAsync(_mapper.Map<GarageExpertiseManagement>(Model))));
        }

        [HttpPut("ExpertiseManagement")]
        public async Task<IActionResult> Update(GarageExpertiseManagementDTO Model)
        {
            string LogoPath = "/Images/Garage/ExpertiseManagement/";
            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }
            return Ok(_mapper.Map<GarageExpertiseManagementDTO>(await _service.UpdateGarageExpertiseManagementAsync(_mapper.Map<GarageExpertiseManagement>(Model))));
        }

        [HttpDelete("ExpertiseManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageExpertiseManagementDTO>(await _service.ArchiveGarageExpertiseManagementAsync(Id)));
        }

        [HttpDelete("ExpertiseManagement/Delete/{Id}")]
        public IActionResult Delete(long Id)
        {
            return Ok(_service.DeleteGarageExpertiseManagementAsync(Id));
        }
    }

}
