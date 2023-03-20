using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Interfaces.IServices.Domains;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantImageController : ControllerBase
    {
        private readonly IRestaurantImageService _service;
        private readonly IFTPUpload _fTPUpload;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantImageController> _logger;

        public RestaurantImageController(IRestaurantImageService service,
            IMapper mapper, 
            IFTPUpload fTPUpload
            , ILogger<RestaurantImageController> logger)
        {
            _service = service;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
            _logger = logger;
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            await _service.ArchiveRestaurantImageAsync(Id);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetByPathAsync(string Path)
        {
            IEnumerable<RestaurantImagesDTO> cities = _mapper.Map<IEnumerable<RestaurantImagesDTO>>(await _service.GetImageByPath(Path));
            return Ok(cities.FirstOrDefault());
        }

        [Authorize]
        [HttpGet("ByRestaurant/{RestaurantId}")]
        public async Task<IActionResult> GetByRestaurantAsync(long RestaurantId)
        {
            IEnumerable<RestaurantImagesDTO> List = _mapper.Map<IEnumerable<RestaurantImagesDTO>>(await _service.GetImageByRestaurant(RestaurantId));
            return Ok(List);
        }
    }
}
