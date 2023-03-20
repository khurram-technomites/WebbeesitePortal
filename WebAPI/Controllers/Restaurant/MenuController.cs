using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _service;
        private readonly IMapper _mapper;
        private readonly IFTPUpload _fTPUpload;

        public MenuController(IMenuService service, IMapper mapper, IFTPUpload fTPUpload)
        {
            _service = service;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("GetAll/{restaurantId}")]
        public async Task<IActionResult> GetAll(long restaurantId)
        {
            return Ok(_mapper.Map<IEnumerable<MenuDTO>>(await _service.GetAllAsync(restaurantId)));
        }

        [HttpGet("General")]
        public async Task<IActionResult> GetGeneralMenu()
        {
            IEnumerable<MenuDTO> menu = _mapper.Map<IEnumerable<MenuDTO>>(await _service.GetGeneralMenuAsync());
            return Ok(menu.FirstOrDefault());
        }

        [HttpGet("GetAll/RestaurantBranches/{branchId}/{id}")]
        public async Task<IActionResult> GetAllByBranchId(long branchId , long id = 0)
        {
            return Ok(_mapper.Map<IEnumerable<MenuDTO>>(await _service.GetAllByBranchIdAsync(branchId , id)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<MenuDTO> List = _mapper.Map<IEnumerable<MenuDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Add(MenuDTO Model)
        {
            var index = _mapper.Map<MenuDTO>(await _service.AddMenuAsync(_mapper.Map<Menu>(Model)));
            return Ok(index);
        }

        [HttpPut]
        public async Task<IActionResult> Update(MenuDTO Model)
        {
            return Ok(_mapper.Map<MenuDTO>(await _service.UpdateMenuAsync(_mapper.Map<Menu>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<MenuDTO>(await _service.ArchiveMenuAsync(Id)));
        }

        [HttpPost("SavePositions")]
        public async Task<IActionResult> SavePosition(MenuDTO Model)
        {
            IEnumerable<Menu> item = await _service.GetByIdAsync(Model.Id);
            Menu menu = item.FirstOrDefault();
            menu.Position = Model.Position;

            return Ok(await _service.UpdateMenuAsync(menu));
        }

        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<Menu> item = await _service.GetByIdAsync(Id);
            Menu make = item.FirstOrDefault();

            if (make.Status == Enum.GetName(typeof(Status), Status.Active))
                make.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                make.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _service.UpdateMenuAsync(make));
        }


    }
}
