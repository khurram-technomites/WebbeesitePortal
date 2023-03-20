using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.SparePartsDealer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.SparePartsDealer
{
    [Route("api/SparePartsDealerInventorySpecifications")]
    [ApiController]
    public class SparePartsDealerInventorySpecificationsController : ControllerBase
    {
        private readonly ISparePartDealerSpecificationService _service;
        private readonly IFTPUpload _ftpUpload;
        private readonly IMapper _mapper;

        public SparePartsDealerInventorySpecificationsController(ISparePartDealerSpecificationService service, IMapper mapper, IFTPUpload ftpUpload)
        {
            _service = service;
            _ftpUpload = ftpUpload;
            _mapper = mapper;
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdt(long Id)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartsDealerSpecificationsDTO>>(await _service.GetById(Id)));
        }

        [HttpGet("CarMake/{CarMakeId}")]
        public async Task<IActionResult> GetByCarMakeId(long CarMakeId)
        {
            IEnumerable<SparePartsDealerSpecificationsDTO> List = _mapper.Map<IEnumerable<SparePartsDealerSpecificationsDTO>>(await _service.GetBySparePartsCarMakeId(CarMakeId));
            return Ok(List.FirstOrDefault());
        }
        [HttpGet("CarModel/{CarModelId}")]
        public async Task<IActionResult> GetByCarModelId(long CarModelId)
        {
            IEnumerable<SparePartsDealerSpecificationsDTO> List = _mapper.Map<IEnumerable<SparePartsDealerSpecificationsDTO>>(await _service.GetBySparePartsCarModelId(CarModelId));
            return Ok(List.FirstOrDefault());
        }
        [HttpGet("SparePartsDealer/{SparePartsDealerId}")]
        public async Task<IActionResult> GetBySparePartsDealerId(long SparePartsDealerId)
        {
            IEnumerable<SparePartsDealerSpecificationsDTO> List = _mapper.Map<IEnumerable<SparePartsDealerSpecificationsDTO>>(await _service.GetBySparePartsDealerId(SparePartsDealerId));
            return Ok(List.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> AddSparePartsDealerInventorySpecifications(SparePartsDealerSpecificationsDTO Model)
        {
            return Ok(_mapper.Map<SparePartsDealerSpecificationsDTO>(await _service.AddSparePartsDealerInventorySpecificationAsync(_mapper.Map<DealerInventorySpecification>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSparePartsDealerInventorySpecifications(SparePartsDealerDocumentDTO Model)
        {
            return Ok(_mapper.Map<SparePartsDealerSpecificationsDTO>(await _service.UpdateSparePartsDealerInventorySpecificationAsync(_mapper.Map<DealerInventorySpecification>(Model))));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteSparePartsDealerInventorySpecifications(long Id)
        {
            await _service.DeleteSparePartsDealerInventorySpecificationAsync(Id);

            return Ok();
        }

    }
}
