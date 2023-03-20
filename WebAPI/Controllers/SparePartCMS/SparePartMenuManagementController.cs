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
using System.Linq;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartMenuManagementController : ControllerBase
    {
        private readonly ISparePartMenuManagementService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;
        public SparePartMenuManagementController(ISparePartMenuManagementService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("MenuManagement")]
        public async Task<IActionResult> GetAllGarageMenuManagement()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartMenuManagementDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("MenuManagement/{Id}")]
        public async Task<IActionResult> GetAllById(long id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartMenuManagementDTO>>(await _service.GetSparePartMenuManagementByIdAsync(id)));
        }

        [HttpGet("{Id}/MenuManagement")]
        public async Task<IActionResult> GetAllBySparePartId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartMenuManagementDTO>>(await _service.GetSparePartMenuManagementBySparePartIdAsync(Id)));
        }

        [HttpGet("Menu/{Id}/Management")]
        public async Task<IActionResult> GetAllByGarageMenuId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartMenuManagementDTO>>(await _service.GetSparePartMenuManagementByMenuIdAsync(Id)));
        }

        [HttpPost("MenuManagement")]
        public async Task<IActionResult> Add(SparePartMenuManagementDTO Model)
        {

            return Ok(_mapper.Map<SparePartMenuManagementDTO>(await _service.AddSparePartMenuManagementAsync(_mapper.Map<SparePartMenuManagement>(Model))));
        }

        [HttpPut("MenuManagement")]
        public async Task<IActionResult> Update(SparePartMenuManagementDTO Model)
        {
            return Ok(_mapper.Map<SparePartMenuManagementDTO>(await _service.UpdateSparePartMenuManagementAsync(_mapper.Map<SparePartMenuManagement>(Model))));
        }

        [HttpDelete("MenuManagement/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartMenuManagementDTO>(await _service.ArchiveSparePartMenuManagementAsync(Id)));
        }

        [HttpPost("MenuManagement/SavePositions")]
        public async Task<IActionResult> SavePosition(SparePartMenuManagementDTO Model)
        {
            IEnumerable<SparePartMenuManagement> item = await _service.GetSparePartMenuManagementByIdAsync(Model.Id);
            SparePartMenuManagement menu = item.FirstOrDefault();
            menu.Position = Model.Position;

            return Ok(await _service.UpdateSparePartMenuManagementAsync(menu));
        }
    }
}
