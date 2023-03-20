using AutoMapper;
using HelperClasses.DTOs;
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
    public class ItemOptionValueController : ControllerBase
    {
        private readonly IItemOptionValueService _service;
        private readonly IMapper _mapper;

        public ItemOptionValueController(IItemOptionValueService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("GetAll/ItemOptions/{itemOptionId}")]
        public async Task<IActionResult> GetAll(long itemOptionId)
        {
            return Ok(_mapper.Map<IEnumerable<ItemOptionValueDTO>>(await _service.GetAllAsync(itemOptionId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<ItemOptionValueDTO> List = _mapper.Map<IEnumerable<ItemOptionValueDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpGet("GetByName/{Name}/ItemOptions/{ItemOptionId}")]
        public async Task<IActionResult> GetByName(long ItemOptionId , string Name)
        {
            ItemOptionValueDTO values = _mapper.Map<ItemOptionValueDTO>(await _service.GetByName(ItemOptionId , Name));
            return Ok(values);
        }


        [HttpPost]
        public async Task<IActionResult> Add(ItemOptionValueDTO Model)
        {
            return Ok(_mapper.Map<ItemOptionValueDTO>(await _service.AddItemOptionValueAsync(_mapper.Map<ItemOptionValue>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ItemOptionValueDTO Model)
        {
            return Ok(_mapper.Map<ItemOptionValueDTO>(await _service.UpdateItemOptionValueAsync(_mapper.Map<ItemOptionValue>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<ItemOptionValueDTO>(await _service.ArchiveItemOptionValueAsync(Id)));
        }
    }
}
