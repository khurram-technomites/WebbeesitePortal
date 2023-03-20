using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.ResponseWrapper;

namespace WebAPI.Controllers
{
    [Route("api/Map")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IMapService _mapService;

        public MapController(IMapService mapService)
        {
            _mapService = mapService;
        }

        [HttpGet("Places")]
        public async Task<IActionResult> Places([FromQuery] string Place)
        {
            return Ok(new SuccessResponse<object>("", await _mapService.GetPlaces(Place)));
        }

        [HttpGet("Place/Detail/{PlaceId}")]
        public async Task<IActionResult> PlaceDetail(string PlaceId)
        {
            return Ok(new SuccessResponse<object>("", await _mapService.GetPlaceDetails(PlaceId)));
        }
    }
}
