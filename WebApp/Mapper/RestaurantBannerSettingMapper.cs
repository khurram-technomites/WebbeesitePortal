using AutoMapper;
using HelperClasses.DTOs;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantBannerSettingMapper : Profile
    {
        public RestaurantBannerSettingMapper()
        {
            CreateMap<RestaurantBannerSettingDTO, RestaurantBannerSettingViewModel>();
            CreateMap<RestaurantBannerSettingViewModel, RestaurantBannerSettingDTO>();
        }
    }
}
