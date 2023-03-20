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
    public class ItemOptionController : ControllerBase
    {
        private readonly IItemOptionService _service;
        private readonly IMapper _mapper;

        public ItemOptionController(IItemOptionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("GetAll/Items/{itemId}")]
        public async Task<IActionResult> GetAll(long itemId)
        {
            return Ok(_mapper.Map<IEnumerable<ItemOptionDTO>>(await _service.GetAllAsync(itemId)));
        }

        [HttpGet("GetMainPrice/Items/{ItemId}/Option/{ItemOptionId}")]
        public async Task<IActionResult> GetMainPrice(long ItemId, long ItemOptionId = 0)
        {
            return Ok(_mapper.Map<IEnumerable<ItemOptionDTO>>(await _service.GetMainPriceAsync(ItemId, ItemOptionId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<ItemOptionDTO> List = _mapper.Map<IEnumerable<ItemOptionDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpGet("GetByName/{Name}/Items/{ItemId}")]
        public async Task<IActionResult> GetById(long ItemId , string Name)
        {
            ItemOptionDTO option = _mapper.Map<ItemOptionDTO>(await _service.GetByName(ItemId , Name));
            return Ok(option);
        }

        [HttpGet("Items/{ItemId}")]
        public async Task<IActionResult> GetByItemId(long ItemId)
        {
            IEnumerable<ItemOptionDTO> List = _mapper.Map<IEnumerable<ItemOptionDTO>>(await _service.GetByItemIdAsync(ItemId));
            return Ok(List);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ItemOptionDTO Model)
        {
            return Ok(_mapper.Map<ItemOptionDTO>(await _service.AddItemOptionAsync(_mapper.Map<ItemOption>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ItemOptionDTO Model)
        {
            return Ok(_mapper.Map<ItemOptionDTO>(await _service.UpdateItemOptionAsync(_mapper.Map<ItemOption>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<ItemOptionDTO>(await _service.ArchiveItemOptionAsync(Id)));
        }

     
    }
}
