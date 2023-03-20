using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantCardSchemeMapper : Profile
    {
        public RestaurantCardSchemeMapper()
        {
            CreateMap<RestaurantCardScheme, RestaurantCardSchemeDTO>();
            CreateMap<RestaurantCardSchemeDTO, RestaurantCardScheme>();
        }
    }
}
