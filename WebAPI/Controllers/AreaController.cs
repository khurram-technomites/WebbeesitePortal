using AutoMapper;
using HelperClasses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AreaController : ControllerBase
    {
        private readonly IAreaService _areaService;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;

        public AreaController(IAreaService AreaService, IMapper mapper, IFTPUpload fTPUpload)
        {
            _areaService = AreaService;
            _fTPUpload = fTPUpload;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<IEnumerable<AreaDTO>>(await _areaService.GetAllAreasAsync()));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<AreaDTO> List = _mapper.Map<IEnumerable<AreaDTO>>(await _areaService.GetAreaByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Add(AreaDTO Model)
        {
            return Ok(_mapper.Map<AreaDTO>(await _areaService.AddAreaAsync(_mapper.Map<Areas>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(AreaDTO Model)
        {
            IEnumerable<Areas> List = await _areaService.GetAreaByIdAsync(Model.Id);
            Areas Area = List.FirstOrDefault();            

            Areas model = _mapper.Map(Model, Area);

            return Ok(_mapper.Map<AreaDTO>(await _areaService.UpdateAreaAsync(_mapper.Map<Areas>(model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<AreaDTO>(await _areaService.ArchiveAreaAsync(Id)));
        }
    }
}
