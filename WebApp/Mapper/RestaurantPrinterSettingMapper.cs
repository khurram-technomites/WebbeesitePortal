using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantPrinterSettingMapper:Profile
    {
        public RestaurantPrinterSettingMapper()
        {
            CreateMap<RestaurantPrinterSettingDTO, RestaurantPrinterSettingViewModel>();
            CreateMap<RestaurantPrinterSettingViewModel, RestaurantPrinterSettingDTO>();
        }
    }
}
