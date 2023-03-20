using AutoMapper;
using HelperClasses.DTOs.Menu;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemOptionController : ControllerBase
    {
        private readonly IMenuItemOptionService _service;
        private readonly IMapper _mapper;

        public MenuItemOptionController(IMenuItemOptionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("GetAll/MenuItems/{MenuItemId}")]
        public async Task<IActionResult> GetAll(long MenuItemId)
        {
            return Ok(_mapper.Map<IEnumerable<MenuItemOptionDTO>>(await _service.GetAllAsync(MenuItemId)));
        }

        [HttpGet("GetMainPrice/MenuItems/{MenuItemId}/Option/{MenuItemOptionId}")]
        public async Task<IActionResult> GetMainPrice(long MenuItemId , long MenuItemOptionId = 0)
        {
            return Ok(_mapper.Map<IEnumerable<MenuItemOptionDTO>>(await _service.GetMainPriceAsync(MenuItemId , MenuItemOptionId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<MenuItemOptionDTO> List = _mapper.Map<IEnumerable<MenuItemOptionDTO>>(await _service.GetById(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Add(MenuItemOptionDTO Model)
        {
            return Ok(_mapper.Map<MenuItemOptionDTO>(await _service.AddMenuItemOptionAsync(_mapper.Map<MenuItemOption>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(MenuItemOptionDTO Model)
        {
            return Ok(_mapper.Map<MenuItemOptionDTO>(await _service.UpdateMenuItemOptionAsync(_mapper.Map<MenuItemOption>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<MenuItemOptionDTO>(await _service.ArchiveMenuItemOptionAsync(Id)));
        }

        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete(long Id)
        {
            return Ok( _service.DeleteMenuItemOption(Id));
        }


    }
}
