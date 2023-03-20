using AutoMapper;
using HelperClasses.DTOs;
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
    public class RestaurantBannerSettingController : ControllerBase
    {
        private readonly IRestaurantBannerSettingService _service;
        private readonly IMapper _mapper;
        private readonly IFTPUpload _fTPUpload;

        public RestaurantBannerSettingController(IRestaurantBannerSettingService service, IMapper mapper, IFTPUpload fTPUpload)
        {
            _service = service;
            _mapper = mapper;
            _fTPUpload = fTPUpload;
        }

        [HttpGet("GetAll/Restaurants/{restaurantId}")]
        public async Task<IActionResult> GetAll(long restaurantId)
        {
            return Ok(_mapper.Map<IEnumerable<RestaurantBannerSettingDTO>>(await _service.GetAllAsync(restaurantId)));
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(long Id)
        {
            IEnumerable<RestaurantBannerSettingDTO> List = _mapper.Map<IEnumerable<RestaurantBannerSettingDTO>>(await _service.GetByIdAsync(Id));
            return Ok(List.FirstOrDefault());
        }

        [HttpGet("Restaurants/{restaurantId}/Type/{type}")]
        public async Task<IActionResult> GetBannerByType(long restaurantId, string type)
        {
            IEnumerable<RestaurantBannerSettingDTO> List = _mapper.Map<IEnumerable<RestaurantBannerSettingDTO>>(await _service.GetBannerByType(restaurantId, type));
            return Ok(List);
        }


        [HttpPost]
        public async Task<IActionResult> Add(RestaurantBannerSettingDTO Model)
        {

            string LogoPath = "/Images/Restaurant/RestaurantBannerSetting/";
            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }


            return Ok(_mapper.Map<RestaurantBannerSettingDTO>(await _service.AddRestaurantBannerSettingAsync(_mapper.Map<RestaurantBannerSetting>(Model))));
        }

        [HttpPut]
        public async Task<IActionResult> Update(RestaurantBannerSettingDTO Model)
        {
            var list = await _service.GetBannerByType(Model.RestaurantId, Model.BannerType);

            if (list.Any())
                foreach (var recod in list)
                    await _service.DeleteRestaurantBannerSettingAsync(recod.Id);


            string LogoPath = "/Images/Restaurant/RestaurantBannerSetting/";
            if (!string.IsNullOrEmpty(Model.ImagePath))
            {
                if (_fTPUpload.MoveFile(Model.ImagePath, ref LogoPath))
                {
                    Model.ImagePath = LogoPath;
                }
            }
            return Ok(_mapper.Map<RestaurantBannerSettingDTO>(await _service.UpdateRestaurantBannerSettingAsync(_mapper.Map<RestaurantBannerSetting>(Model))));
        }
        [HttpPut("MenuImage/{Id}")]
        public async Task<IActionResult> MenuImage(long Id)
        {
            IEnumerable<RestaurantBannerSetting> List = await _service.GetByIdAsync(Id);

            RestaurantBannerSetting restaurant = List.FirstOrDefault();
            restaurant.ImagePath = null;
            return Ok(_mapper.Map<RestaurantBannerSettingDTO>(await _service.UpdateRestaurantBannerSettingAsync(restaurant)));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Archive(long Id)
        {
            await _service.ArchiveRestaurantBannerSettingAsync(Id);
            return Ok();
        }
    }
}
