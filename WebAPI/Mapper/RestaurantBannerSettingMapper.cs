using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class RestaurantBannerSettingMapper : Profile
    {
        public RestaurantBannerSettingMapper() 
        {
            CreateMap<RestaurantBannerSetting, RestaurantBannerSettingDTO>();
            CreateMap<RestaurantBannerSettingDTO, RestaurantBannerSetting>();

            CreateMap<RestaurantBannerSetting, LandingPromotionBannerDTO>();
        }
    }
}
