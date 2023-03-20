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
    public class MenuItemOptionValuesController : ControllerBase
    {
        private readonly IMenuItemOptionValueService _service;
        private readonly IMapper _mapper;

        public MenuItemOptionValuesController(IMenuItemOptionValueService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("GetAll/MenuItemOptions/{MenuItemOptionId}")]
        public async Task<IActionResult> GetAll(long MenuItemId)
        {
            return Ok(_mapper.Map<IEnumerable<MenuItemOptionValueDTO>>(await _service.GetAllAsync(MenuItemId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<MenuItemOptionValueDTO> List = _mapper.Map<IEnumerable<MenuItemOptionValueDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }


        [HttpPost]
        public async Task<IActionResult> Add(MenuItemOptionValueDTO Model)
        {
            return Ok(_mapper.Map<MenuItemOptionValueDTO>(await _service.AddMenuItemOptionValueAsync(_mapper.Map<MenuItemOptionValue>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(MenuItemOptionValueDTO Model)
        {
            return Ok(_mapper.Map<MenuItemOptionValueDTO>(await _service.UpdateMenuItemOptionValueAsync(_mapper.Map<MenuItemOptionValue>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<MenuItemOptionValueDTO>(await _service.ArchiveMenuItemOptionValueAsync(Id)));
        }


        [HttpDelete("Delete/{Id}")]
        public IActionResult Delete(long Id)
        {
            return Ok(_service.DeleteMenuItemOptionValue(Id));
        }


    }
}
