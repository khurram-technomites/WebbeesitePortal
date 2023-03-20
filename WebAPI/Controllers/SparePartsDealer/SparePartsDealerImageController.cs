using AutoMapper;
using HelperClasses.DTOs.SparePartsDealer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
namespace WebAPI.Controllers.SparePartsDealer
{
    [Route("api/SparePartsDealerImage")]
    [ApiController]
    public class SparePartsDealerImageController : ControllerBase
    {
        private readonly IDealerImageService _dealerImageService;
        private readonly IMapper _mapper;

        public SparePartsDealerImageController(IDealerImageService dealerImageService, IMapper mapper)
        {
            _dealerImageService = dealerImageService;
            _mapper = mapper;
        }

        [HttpGet("{sparePartsDealerId}/Image")]
        public async Task<IActionResult> GetBySpareParts(long sparePartsDealerId)
        {
            return Ok(_mapper.Map<IEnumerable<SparePartDealerImagesDTO>>(await _dealerImageService.GetDealerImageBySparePartsDealerIdAsync(sparePartsDealerId)));
        }

        [HttpDelete("Image/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            await _dealerImageService.DeleteDealerImageAsync(Id);
            return Ok();
        }
    }
}
