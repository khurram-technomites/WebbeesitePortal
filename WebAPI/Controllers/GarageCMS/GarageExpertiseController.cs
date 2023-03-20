using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    public class GarageExpertiseController : ControllerBase
    {
        private readonly IGarageExpertiseService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        public GarageExpertiseController(IGarageExpertiseService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet("Expertise")]
        public async Task<IActionResult> GetAllGarageExpertise()
        {
            return Ok(_mapper.Map<IEnumerable<GarageExpertiseDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("Expertise/{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageExpertiseDTO>>(await _service.GetGarageExpertiseByIdAsync(Id)));
        }
       
        [HttpGet("Expertise/ByManagement/{Id}")]
        public async Task<IActionResult> GetAllByManagementId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageExpertiseDTO>>(await _service.GetGarageExpertiseByGarageExpertiseManagementIdAsync(Id)));
        }

        [HttpPost("Expertise")]
        public async Task<IActionResult> Add(GarageExpertiseDTO Model)
        {

            return Ok(_mapper.Map<GarageExpertiseDTO>(await _service.AddGarageExpertiseAsync(_mapper.Map<GarageExpertise>(Model))));
        }

        [HttpPut("Expertise")]
        public async Task<IActionResult> Update(GarageExpertiseDTO Model)
        {
            return Ok(_mapper.Map<GarageExpertiseDTO>(await _service.UpdateGarageExpertiseAsync(_mapper.Map<GarageExpertise>(Model))));
        }

        [HttpDelete("Expertise/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageExpertiseDTO>(await _service.ArchiveGarageExpertiseAsync(Id)));
        }

        [HttpDelete("Expertise/Delete/{Id}")]
        public IActionResult Delete(long Id)
        {
            return Ok( _service.DeleteGarageExpertiseAsync(Id));
        }
    }
}
