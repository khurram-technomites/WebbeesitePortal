using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.SparePartsDealer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.SparePartsDealer
{
    [Route("api/SparePartsDealerSchedule")]
    [ApiController]
    public class SparePartsDealerScheduleController : ControllerBase
    {
        private readonly ISparePartDealerScheduleService _service;
        private readonly IMapper _mapper;

        public SparePartsDealerScheduleController(ISparePartDealerScheduleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }
        [HttpGet("GetAll/SparePartsDealer/{SparePartsDealerId}")]
        public async Task<IActionResult> GetAll(long SparePartsDealerId)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartsDealerScheduleDTO>>(await _service.GetSparePartDealerScheduleByDealer(SparePartsDealerId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<SparePartsDealerScheduleDTO> List = _mapper.Map<IEnumerable<SparePartsDealerScheduleDTO>>(await _service.GetSparePartDealerScheduleById(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Add(SparePartsDealerScheduleDTO Model)
        {

            IEnumerable<SparePartsDealerScheduleDTO> isExist = _mapper.Map<IEnumerable<SparePartsDealerScheduleDTO>>(await _service.GetScheduleByDay(Model.Day, Model.OpeningTime, Model.ClosingTime, Model.SparePartsDealerId));
            if (!isExist.Any())
            {
                return Ok(_mapper.Map<SparePartsDealerScheduleDTO>(await _service.AddSparePartDealerScheduleAsync(_mapper.Map<DealerSchedule>(Model))));
            }

            return Conflict();
        }

        [HttpPut]
        public async Task<IActionResult> Update(SparePartsDealerScheduleDTO Model)
        {

            IEnumerable<DealerSchedule> isBranchExist = await _service.GetScheduleByDay(Model.Day, Model.OpeningTime, Model.ClosingTime, Model.SparePartsDealerId, Model.Id);
            if (!isBranchExist.Any())
            {
                return Ok(_mapper.Map<SparePartsDealerScheduleDTO>(await _service.UpdateSparePartDealerBranchScheduleAsync(_mapper.Map<DealerSchedule>(Model))));
            }

            return Conflict();
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            await _service.ArchiveSparePartDealerScheduleAsync(Id);
            return Ok();
        }

    }
}
