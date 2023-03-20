using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/Restaurant")]
    [ApiController]
    public class RestaurantCardSchemeController : ControllerBase
    {
        private readonly IRestaurantCardSchemeService _restaurantCardSchemeService;
        private readonly IMapper _mapper;
        public RestaurantCardSchemeController(IRestaurantCardSchemeService restaurantCardSchemeService, IMapper mapper)
        {
            _restaurantCardSchemeService = restaurantCardSchemeService;
            _mapper = mapper;
        }

        [HttpGet("CardScheme")]
        public async Task<IActionResult> GellAll()
        {
            var CardScheme = _mapper.Map<IEnumerable<RestaurantCardSchemeDTO>>(await _restaurantCardSchemeService.GetAllAsync());
            return Ok(CardScheme);
        }

        [HttpGet("CardScheme/{Id}")]
        public async Task<IActionResult> GetByIdAsync(long Id)
        {
            var CardScheme = _mapper.Map<IEnumerable<RestaurantCardSchemeDTO>>(await _restaurantCardSchemeService.GetByIdAsync(Id));
            return Ok(CardScheme.FirstOrDefault());
        }

        [HttpGet("{Id}/CardScheme")]
        public async Task<IActionResult> GetByRestaurantIdAsync(long Id)
        {
            var CardScheme = _mapper.Map<IEnumerable<RestaurantCardSchemeDTO>>(await _restaurantCardSchemeService.GetByRestaurantIdAsync(Id));
            return Ok(CardScheme);
        }

        [HttpGet("CardScheme/ByBranch/{Id}")]
        public async Task<IActionResult> GetByBranchIdAsync(long Id)
        {
            var CardScheme = _mapper.Map <IEnumerable<RestaurantCardSchemeDTO>>(await _restaurantCardSchemeService.GetByRestaurantBranchIdAsync(Id));
            return Ok(CardScheme);
        }

        [HttpPost("CardScheme")]
        public async Task<IActionResult> AddCardScheme(RestaurantCardSchemeDTO Model)
        {
            return Ok(_mapper.Map<RestaurantCardSchemeDTO>(await _restaurantCardSchemeService.AddRestaurantCardSchemeAsync(_mapper.Map<RestaurantCardScheme>(Model))));
        }
        [HttpPut("CardScheme")]
        public async Task<IActionResult> UpdateCardScheme(RestaurantCardSchemeDTO Model)
        {
            IEnumerable<RestaurantCardScheme> List =await _restaurantCardSchemeService.GetByIdAsync(Model.Id);
            RestaurantCardScheme restaurantCardScheme = List.FirstOrDefault();
            restaurantCardScheme=_mapper.Map(Model,restaurantCardScheme);

            return Ok(_mapper.Map<RestaurantCardSchemeDTO>(await _restaurantCardSchemeService.UpdateRestaurantCardSchemeAsync(_mapper.Map<RestaurantCardScheme>(restaurantCardScheme))));

        }

        [HttpDelete("CardScheme/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<RestaurantCardSchemeDTO>(await _restaurantCardSchemeService.ArchiveRestaurantCardSchemeAsync(Id)));
        }




    }
}
