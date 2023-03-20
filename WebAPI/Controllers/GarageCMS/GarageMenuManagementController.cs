using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    public class GarageMenuManagementController : ControllerBase
    {
        private readonly IGarageMenuManagementService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        public GarageMenuManagementController(IGarageMenuManagementService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
        }
        [HttpGet("MenuManagement")]
        public async Task<IActionResult> GetAllGarageMenuManagement()
        {
            return Ok(_mapper.Map<IEnumerable<GarageMenuManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("MenuManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageMenuManagementDTO>>(await _service.GetGarageMenuManagementByIdAsync(id)));
        }

        [HttpGet("{Id}/MenuManagement")]
        public async Task<IActionResult> GetAllByGarageId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageMenuManagementDTO>>(await _service.GetGarageMenuManagementByGaragedIdAsync(Id)));
        }

        [HttpGet("Menu/{Id}/Management")]
        public async Task<IActionResult> GetAllByGarageMenuId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<GarageMenuManagementDTO>>(await _service.GetGarageMenuManagementByMenuIdAsync(Id)));
        }

        [HttpPost("MenuManagement")]
        public async Task<IActionResult> Add(GarageMenuManagementDTO Model)
        {

            return Ok(_mapper.Map<GarageMenuManagementDTO>(await _service.AddGarageMenuManagementAsync(_mapper.Map<GarageMenuManagement>(Model))));
        }

        [HttpPut("MenuManagement")]
        public async Task<IActionResult> Update(GarageMenuManagementDTO Model)
        {
            return Ok(_mapper.Map<GarageMenuManagementDTO>(await _service.UpdateGarageMenuManagementAsync(_mapper.Map<GarageMenuManagement>(Model))));
        }

        [HttpDelete("MenuManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageMenuManagementDTO>(await _service.ArchiveGarageMenuManagementAsync(Id)));
        }

        [HttpPost("MenuManagement/SavePositions")]
        public async Task<IActionResult> SavePosition(GarageMenuManagementDTO Model)
        {
            IEnumerable<GarageMenuManagement> item = await _service.GetGarageMenuManagementByIdAsync(Model.Id);
            GarageMenuManagement menu = item.FirstOrDefault();
            menu.Position = Model.Position;

            return Ok(await _service.UpdateGarageMenuManagementAsync(menu));
        }
    }
}
