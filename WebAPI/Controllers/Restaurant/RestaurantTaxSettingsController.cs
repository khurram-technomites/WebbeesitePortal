using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Controllers.Restaurant
{
    [Route("api/Restaurant")]
    [ApiController]
    public class RestaurantTaxSettingsController : ControllerBase
    {
        private readonly IRestaurantTaxSettingService _restaurantTaxSettingService;
        private readonly IMapper _mapper;

        public RestaurantTaxSettingsController(IRestaurantTaxSettingService restaurantTaxSettingService, IMapper mapper)
        {
            _restaurantTaxSettingService = restaurantTaxSettingService;
            _mapper = mapper;
        }

        [HttpGet("TaxSettings")]
        public async Task<IActionResult> GetAll()
        {
            var taxSettings=_mapper.Map<IEnumerable<RestaurantTaxSettingDTO>>(await _restaurantTaxSettingService.GetAllAsync());
            return Ok(taxSettings);
        }

        [HttpGet("TaxSettings/{Id}")]
        public async Task<IActionResult> GetByIdAsyn(long Id)
        {
            var taxSettings = _mapper.Map<IEnumerable<RestaurantTaxSettingDTO>>(await _restaurantTaxSettingService.GetByIdAsync(Id));
            return Ok(taxSettings.FirstOrDefault());
        }

        [HttpGet("{Id}/TaxSettings")]
        public async Task<IActionResult> GetByRestaurantIdAsync(long Id)
        {
            var taxSettings = _mapper.Map<IEnumerable<RestaurantTaxSettingDTO>>(await _restaurantTaxSettingService.GetByRestaurantIdAsync(Id));
            return Ok(taxSettings);
        }

        [HttpGet("TaxSettings/ByBranch/{Id}")]
        public async Task<IActionResult> GetByBranchIdAsync(long Id)
        {
            var taxSettings = _mapper.Map<IEnumerable<RestaurantTaxSettingDTO>>(await _restaurantTaxSettingService.GetByRestaurantBranchIdAsync(Id));
            return Ok(taxSettings);
        }
        [HttpPost("TaxSettings")]
        public async Task<IActionResult> AddTaxSettings(RestaurantTaxSettingDTO Model)
        {
            return Ok(_mapper.Map<RestaurantTaxSettingDTO>(await _restaurantTaxSettingService.AddRestaurantTaxSettingAsync(_mapper.Map<RestaurantTaxSetting>(Model))));
        }
        [HttpPut("TaxSettings")]
        public async Task<IActionResult> UpdateTaxSettings(RestaurantTaxSettingDTO Model)
        {
            IEnumerable<RestaurantTaxSetting> List = await _restaurantTaxSettingService.GetByIdAsync(Model.Id);
            RestaurantTaxSetting restaurantTaxSetting = List.FirstOrDefault();
            restaurantTaxSetting=_mapper.Map(Model,restaurantTaxSetting);

            return Ok(_mapper.Map<RestaurantTaxSettingDTO>(await _restaurantTaxSettingService.UpdateRestaurantTaxSettingAsync(_mapper.Map<RestaurantTaxSetting>(restaurantTaxSetting))));
        }

        [HttpDelete("TaxSettings/{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            return Ok(_mapper.Map<RestaurantTaxSettingDTO>(await _restaurantTaxSettingService.ArchiveRestaurantTaxSettingAsync(Id)));
        }

    }
}
