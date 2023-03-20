using AutoMapper;
using HelperClasses.DTOs.Order;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantOrderDetailController : ControllerBase
    {
        private readonly IOrderDetailService _service;
        private readonly IMapper _mapper;

        public RestaurantOrderDetailController(IOrderDetailService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("GetAll/Orders/{OrderId}")]
        public async Task<IActionResult> GetAll(long OrderId)
        {
            return Ok(_mapper.Map<IEnumerable<OrderDetailDTO>>(await _service.GetAllAsync(OrderId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<OrderDetailDTO> List = _mapper.Map<IEnumerable<OrderDetailDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

    }
}
