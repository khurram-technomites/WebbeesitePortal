using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Expertise")]
    [ApiController]
    public class ExpertiseController : ControllerBase
    {
        private readonly IExpertiseService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        public ExpertiseController(IExpertiseService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllExpertise()
        {
            return Ok(_mapper.Map<IEnumerable<ExpertiseDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<ExpertiseDTO>>(await _service.GetExpertiseByIdAsync(Id)));
        }

        [HttpPost]
        public async Task<IActionResult> Add(ExpertiseDTO Model)
        {

            return Ok(_mapper.Map<ExpertiseDTO>(await _service.AddExpertiseAsync(_mapper.Map<Expertise>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ExpertiseDTO Model)
        {
            return Ok(_mapper.Map<ExpertiseDTO>(await _service.UpdateExpertiseAsync(_mapper.Map<Expertise>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<ExpertiseDTO>(await _service.ArchiveExpertiseAsync(Id)));
        }

        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<Expertise> expertiseList = await _service.GetExpertiseByIdAsync(Id);
            Expertise expertise = expertiseList.FirstOrDefault();

            if (expertise.Status == Enum.GetName(typeof(Status), Status.Active))
                expertise.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                expertise.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(_mapper.Map<ExpertiseDTO>(await _service.UpdateExpertiseAsync(expertise)));
        }
    }
}
