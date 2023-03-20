using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantTaxSettingMapper:Profile
    {
        public RestaurantTaxSettingMapper()
        {
            CreateMap<RestaurantTaxSettingViewModel, RestaurantTaxSettingDTO>();
            CreateMap<RestaurantTaxSettingDTO, RestaurantTaxSettingViewModel>();
        }
    }
}
