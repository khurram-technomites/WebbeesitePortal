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
    public class SparePartExpertiseController : ControllerBase
    {
        private readonly ISparePartExpertiseService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;
        public SparePartExpertiseController(ISparePartExpertiseService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("Expertise")]
        public async Task<IActionResult> GetAllGarageExpertise()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartExpertiseDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("Expertise/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartExpertiseDTO>>(await _service.GetSparePartExpertiseByIdAsync(Id)));
        }

        [HttpGet("Expertise/ByManagement/{Id}")]
        public async Task<IActionResult> GetAllByManagementId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartExpertiseDTO>>(await _service.GetSparePartExpertiseBySparePartExpertiseManagementIdAsync(Id)));
        }

        [HttpPost("Expertise")]
        public async Task<IActionResult> Add(SparePartExpertiseDTO Model)
        {

            return Ok(_mapper.Map<SparePartExpertiseDTO>(await _service.AddSparePartExpertiseAsync(_mapper.Map<SparePartExpertise>(Model))));
        }

        [HttpPut("Expertise")]
        public async Task<IActionResult> Update(SparePartExpertiseDTO Model)
        {
            return Ok(_mapper.Map<SparePartExpertiseDTO>(await _service.UpdateSparePartExpertiseAsync(_mapper.Map<SparePartExpertise>(Model))));
        }

        [HttpDelete("Expertise/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartExpertiseDTO>(await _service.ArchiveSparePartExpertiseAsync(Id)));
        }

        [HttpDelete("Expertise/Delete/{Id}")]
        public IActionResult Delete(long Id)
        {
            return Ok(_service.DeleteSparePartExpertiseAsync(Id));
        }
    }
}
