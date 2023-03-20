using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.Garage;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers.GarageCMS
{
    [Route("api/Garage")]
    [ApiController]
    [Authorize(Roles = "GarageOwner")]
    public class GarageFAQController : ControllerBase
    {
        private readonly IGarageFAQService _fAQService;
        private readonly IMapper _mapper;

        public GarageFAQController(IGarageFAQService fAQService, IMapper mapper)
        {
            _fAQService = fAQService;
            _mapper = mapper;
        }

        [HttpGet("{GarageId}/FAQ")]
        public async Task<IActionResult> GetByGarage(long GarageId)
        {
            return Ok(_mapper.Map<IEnumerable<GarageFAQDTO>>(await _fAQService.GetFAQByGarageAsync(GarageId)));
        }

        [HttpGet("FAQ/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<GarageFAQDTO> result = _mapper.Map<IEnumerable<GarageFAQDTO>>(await _fAQService.GetFAQByIdAsync(Id));
            return Ok(result.FirstOrDefault());
        }

        [HttpPost("FAQ")]
        public async Task<IActionResult> Insert(GarageFAQDTO Model)
        {
            long count = await _fAQService.MaxCount(Model.GarageId);
            Model.Position = (int)(count + 1);
            return Ok(_mapper.Map<GarageFAQDTO>(await _fAQService.AddFAQAsync(_mapper.Map<GarageFAQ>(Model))));
        }

        [HttpPut("FAQ")]
        public async Task<IActionResult> Update(GarageFAQDTO Model)
        {
            return Ok(_mapper.Map<GarageFAQDTO>(await _fAQService.UpdateFAQAsync(_mapper.Map<GarageFAQ>(Model))));
        }

        [HttpDelete("FAQ/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<GarageFAQDTO>(await _fAQService.ArchiveFAQAsync(Id)));
        }

        [HttpPut("FAQ/SavePositions")]
        public async Task<IActionResult> SavePosition(GarageFAQDTO Model)
        {
            IEnumerable<GarageFAQ> FAQs = await _fAQService.GetFAQByIdAsync(Model.Id);
            GarageFAQ FAQ = FAQs.FirstOrDefault();
            FAQ.Position = Model.Position;

            return Ok(_mapper.Map<GarageFAQDTO>(await _fAQService.UpdateFAQAsync(FAQ)));
        }
    }
}
