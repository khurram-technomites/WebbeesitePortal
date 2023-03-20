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
    [Route("api/Garage/Menu")]
    [ApiController]
    public class GarageMenuController : ControllerBase
    {

        private readonly IGarageMenuService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        public GarageMenuController(IGarageMenuService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGarageMenu()
        {
            return Ok(_mapper.Map<IEnumerable<GarageMenuDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageMenuDTO>>(await _service.GetGarageMenuByIdAsync(Id)));
        }

        [HttpGet("Garages/{GarageId}")]
        public async Task<IActionResult> GetMenuByGarageId(long GarageId)
        {
            return Ok(_mapper.Map<IEnumerable<GarageMenuDTO>>(await _service.GetMenuByGarageId(GarageId)));
        }

        [HttpPost]
        public async Task<IActionResult> Add(GarageMenuDTO Model)
        {

            return Ok(_mapper.Map<GarageMenuDTO>(await _service.AddGarageMenuAsync(_mapper.Map<GarageMenu>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(GarageMenuDTO Model)
        {
            return Ok(_mapper.Map<GarageMenuDTO>(await _service.UpdateGarageMenuAsync(_mapper.Map<GarageMenu>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageMenuDTO>(await _service.ArchiveGarageMenuAsync(Id)));
        }
    }
}
