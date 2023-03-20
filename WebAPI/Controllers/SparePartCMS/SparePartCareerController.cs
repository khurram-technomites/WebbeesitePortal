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
using HelperClasses.DTOs.SparePartCMS;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartCareerController : ControllerBase
    {
        private readonly ISparePartCareerService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;

        public SparePartCareerController(ISparePartCareerService service, IMapper mapper, IUserService userService, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
            _userManager = userManager;
        }

        [HttpGet("Careers")]
        public async Task<IActionResult> GetAllGarageCareers()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartCareerDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("Careers/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartCareerDTO>>(await _service.GetSparePartCareerByIdAsync(Id)));
        }

        [HttpGet("{Id}/Careers")]
        public async Task<IActionResult> GetAllBySparePartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartCareerDTO>>(await _service.GetSparePartCareerBySparePartDealerIdAsync(Id)));
        }
        [HttpPost("Careers")]
        public async Task<IActionResult> Add(SparePartCareerDTO Model)
        {

            return Ok(_mapper.Map<SparePartCareerDTO>(await _service.AddSparePartCareerAsync(_mapper.Map<SparePartCareer>(Model))));
        }

        [HttpPut("Careers")]
        public async Task<IActionResult> Update(SparePartCareerDTO Model)
        {
            return Ok(_mapper.Map<SparePartCareerDTO>(await _service.UpdateSparePartCareerAsync(_mapper.Map<SparePartCareer>(Model))));
        }
        [HttpDelete("Careers/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartCareerDTO>(await _service.ArchiveSparePartCareerAsync(Id)));
        }
    }
}
