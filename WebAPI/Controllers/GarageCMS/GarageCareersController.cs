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

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    public class GarageCareersController : ControllerBase
    {
        private readonly IGarageCareersService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IGarageService _garageService;
        private readonly IFTPUpload _fTPUpload;
        private readonly UserManager<AppUser> _userManager;
        public GarageCareersController(IGarageCareersService service, IMapper mapper, IUserService userService, IGarageService garageService, IFTPUpload fTPUpload, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _garageService = garageService;
            _fTPUpload = fTPUpload;
            _userManager = userManager;
        }

        [HttpGet("Careers")]
        public async Task<IActionResult> GetAllGarageCareers()
        {
            return Ok(_mapper.Map<IEnumerable<GarageCareerDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("Careers/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageCareerDTO>>(await _service.GetGarageCareersByIdAsync(Id)));
        }

        [HttpGet("{Id}/Careers")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageCareerDTO>>(await _service.GetGarageCareersByGarageIdAsync(Id)));
        }
        [HttpPost("Careers")]
        public async Task<IActionResult> Add(GarageCareerDTO Model)
        {

            return Ok(_mapper.Map<GarageCareerDTO>(await _service.AddGarageCareersAsync(_mapper.Map<GarageCareers>(Model))));
        }

        [HttpPut("Careers")]
        public async Task<IActionResult> Update(GarageCareerDTO Model)
        {
            return Ok(_mapper.Map<GarageCareerDTO>(await _service.UpdateGarageCareersAsync(_mapper.Map<GarageCareers>(Model))));
        }
        [HttpDelete("Careers/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageCareerDTO>(await _service.ArchiveGarageCareersAsync(Id)));
        }
    }
}
