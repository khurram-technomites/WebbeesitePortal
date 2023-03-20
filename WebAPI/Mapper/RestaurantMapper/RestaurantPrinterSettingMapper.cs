using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantPrinterSettingMapper : Profile
    {
        public RestaurantPrinterSettingMapper()
        {
            CreateMap<RestaurantPrinterSetting, RestaurantPrinterSettingDTO>();
            CreateMap<RestaurantPrinterSettingDTO, RestaurantPrinterSetting>();

        }
    }
}
