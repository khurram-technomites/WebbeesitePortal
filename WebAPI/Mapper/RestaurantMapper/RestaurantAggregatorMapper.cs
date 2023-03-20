using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantAggregatorMapper : Profile
    {
        public RestaurantAggregatorMapper()
        {
            CreateMap<RestaurantAggregator, RestaurantAggregatorDTO>();
            CreateMap<RestaurantAggregatorDTO, RestaurantAggregator>();

        }
    }
}
