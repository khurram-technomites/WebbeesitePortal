using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantTaxSettingMapper : Profile
    {
        public RestaurantTaxSettingMapper()
        {
            CreateMap<RestaurantTaxSetting, RestaurantTaxSettingDTO>();
            CreateMap<RestaurantTaxSettingDTO, RestaurantTaxSetting>();
        }
    }
}
