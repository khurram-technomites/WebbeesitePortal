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
    public class SparePartMenuController : ControllerBase
    {
        private readonly ISparePartMenuService _service;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ISparePartsDealerService _sparePartsService;
        private readonly IFTPUpload _fTPUpload;
        public SparePartMenuController(ISparePartMenuService service, IMapper mapper, IUserService userService, UserManager<AppUser> userManager, ISparePartsDealerService sparePartsService, IFTPUpload fTPUpload)
        {
            _service = service;
            _mapper = mapper;
            _userService = userService;
            _userManager = userManager;
            _sparePartsService = sparePartsService;
            _fTPUpload = fTPUpload;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGarageMenu()
        {
            return Ok(_mapper.Map<IEnumerable<SparePartMenuDTO>>(await _service.GetAllAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAllById(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartMenuDTO>>(await _service.GetSparePartMenuById(Id)));
        }

        [HttpGet("Menu/BySparePart/{Id}")]
        public async Task<IActionResult> GetMenuBySparepartDealerId(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartMenuDTO>>(await _service.GetSparePartMenuBySparepartDealerId(Id)));
        }

        [HttpPost]
        public async Task<IActionResult> Add(SparePartMenuDTO Model)
        {

            return Ok(_mapper.Map<SparePartMenuDTO>(await _service.AddSparePartMenuAsync(_mapper.Map<SparePartMenu>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(SparePartMenuDTO Model)
        {
            return Ok(_mapper.Map<SparePartMenuDTO>(await _service.UpdateSparePartMenuAsync(_mapper.Map<SparePartMenu>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartMenuDTO>(await _service.ArchiveSparePartMenuAsync(Id)));
        }
    }
}
