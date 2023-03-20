using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RouteGroupController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRouteGroupService _service;
        public RouteGroupController(IMapper mapper, IRouteGroupService service)
        {
            _mapper = mapper;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<RouteGroupDTO>>(await _service.GetAllRouteGroups()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAll(int Id)
        {
            IEnumerable<RouteGroupDTO> List = _mapper.Map<IEnumerable<RouteGroupDTO>>(await _service.GetRouteGroupsById(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpGet("ByGroup/{Id}")]
        public async Task<IActionResult> GetByGroups(int Id)
        {
            return Ok(_mapper.Map<IEnumerable<RouteGroupDTO>>(await _service.GetByGroup(Id)));
        }

        [HttpPost]
        public async Task<IActionResult> AddRangeAsync(IEnumerable<RouteGroupDTO> Entities)
        {
            IEnumerable<RouteGroup> List = _mapper.Map<IEnumerable<RouteGroup>>(Entities);
            return Ok(_mapper.Map<IEnumerable<RouteGroupDTO>>(await _service.AddRangeAsync(List)));
        }
    }
}
