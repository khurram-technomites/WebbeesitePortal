using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using HelperClasses.DTOs.SparePartCMS;

namespace WebAPI.Controllers.SparePartCMS
{
    [Route("api/SpareParts")]
    [ApiController]
    public class SparePartFAQController : ControllerBase
    {
        private readonly ISparePartFAQService _service;
        private readonly IMapper _mapper;

        public SparePartFAQController(ISparePartFAQService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet("{Id}/FAQ")]
        public async Task<IActionResult> GetBySparePart(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartFAQDTO>>(await _service.GetFAQBySparePartAsync(Id)));
        }

        [HttpGet("FAQ/{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<SparePartFAQDTO> result = _mapper.Map<IEnumerable<SparePartFAQDTO>>(await _service.GetFAQByIdAsync(Id));
            return Ok(result.FirstOrDefault());
        }

        [HttpPost("FAQ")]
        public async Task<IActionResult> Insert(SparePartFAQDTO Model)
        {
            long count = await _service.MaxCount(Model.SparePartId);
            Model.Position = (int)(count + 1);
            return Ok(_mapper.Map<SparePartFAQDTO>(await _service.AddFAQAsync(_mapper.Map<SparePartFAQ>(Model))));
        }

        [HttpPut("FAQ")]
        public async Task<IActionResult> Update(SparePartFAQDTO Model)
        {
            return Ok(_mapper.Map<SparePartFAQDTO>(await _service.UpdateFAQAsync(_mapper.Map<SparePartFAQ>(Model))));
        }

        [HttpDelete("FAQ/{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            return Ok(_mapper.Map<SparePartFAQDTO>(await _service.ArchiveFAQAsync(Id)));
        }

        [HttpPut("FAQ/SavePositions")]
        public async Task<IActionResult> SavePosition(SparePartFAQDTO Model)
        {
            IEnumerable<SparePartFAQ> FAQs = await _service.GetFAQByIdAsync(Model.Id);
            SparePartFAQ FAQ = FAQs.FirstOrDefault();
            FAQ.Position = Model.Position;

            return Ok(_mapper.Map<SparePartFAQDTO>(await _service.UpdateFAQAsync(FAQ)));
        }
    }
}
