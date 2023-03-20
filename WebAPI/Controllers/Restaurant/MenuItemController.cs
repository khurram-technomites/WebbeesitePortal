using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Menu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _service;
        private readonly IMapper _mapper;
        private readonly IFTPUpload _fTPUpload;
        public MenuItemController(IMenuItemService service, IMapper mapper, IFTPUpload fTPUpload)
        {
            _service = service;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("GetAll/Menu/{MenuId}")]
        public async Task<IActionResult> GetAll(long MenuId)
        {
            return Ok(_mapper.Map<IEnumerable<MenuItemDTO>>(await _service.GetAllAsync(MenuId)));
        }

        [HttpGet("GetAll/Menu/{MenuId}/Categories")]
        public async Task<IActionResult> GetAllMenuItem(long MenuId)
        {
            return Ok(_mapper.Map<IEnumerable<MenuItemByMenuDTO>>(await _service.GetMenuItemByMenuId(MenuId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<MenuItemDTO> List = _mapper.Map<IEnumerable<MenuItemDTO>>(await _service.GetById(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpGet("MenuItemDetails/{Id}")]
        public async Task<IActionResult> GetDetailsById(long Id)
        {
            IEnumerable<MenuItemDTO> List = _mapper.Map<IEnumerable<MenuItemDTO>>(await _service.GetById(Id));

            return Ok(new SuccessResponse<MenuItemDTO>("Data received successfully." , List.FirstOrDefault()));
        }

        [HttpGet("CheckMainPrice/{Id}")]
        public async Task<bool> CheckMainPrice(long Id)
        {
            return await _service.CheckMainPrice(Id);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MenuItemDTO Model)
        {
            return Ok(_mapper.Map<MenuItemDTO>(await _service.AddMenuItemAsync(_mapper.Map<MenuItem>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(MenuItemDTO Model)
        {
            return Ok(_mapper.Map<MenuItemDTO>(await _service.UpdateMenuItemAsync(_mapper.Map<MenuItem>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<MenuItemDTO>(await _service.ArchiveMenuItemAsync(Id)));
        }

        [HttpPost("SavePositions")]
        public async Task<IActionResult> SavePosition(MenuItemDTO Model)
        {
            IEnumerable<MenuItem> item = await _service.GetById(Model.Id);
            MenuItem menuItem = item.FirstOrDefault();
            menuItem.Position = Model.Position;

            return Ok(await _service.UpdateMenuItemAsync(menuItem));
        }

        [HttpPost("SaveCategoryPositions")]
        public async Task<IActionResult> CategorySavePosition(MenuItemDTO Model)
        {
            IEnumerable<MenuItem> menuItem = await _service.GetAllCategoryByMenuAsync(Model.MenuId , Model.CategoryId);
            if (menuItem.Any())
            {
                foreach (var item in menuItem)
                {
                    item.CategoryPosition = Model.CategoryPosition;
                    await _service.UpdateMenuItemAsync(item);
                }
            }

            return Ok();
        }

        [HttpGet("ToggleStatus/{Id}")]
        public async Task<IActionResult> ToggleStatus(long Id)
        {
            IEnumerable<MenuItem> MenuItem = await _service.GetById(Id);
            MenuItem make = MenuItem.FirstOrDefault();

            if (make.Status == Enum.GetName(typeof(Status), Status.Active))
                make.Status = Enum.GetName(typeof(Status), Status.Inactive);
            else
                make.Status = Enum.GetName(typeof(Status), Status.Active);

            return Ok(await _service.UpdateMenuItemAsync(make));
        }
    }
}
