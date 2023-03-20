using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantMapper : Profile
    {
        public RestaurantMapper()
        {
            CreateMap<Restaurant, RestaurantDTO>()
                .ForMember(x => x.Logo, x => x.MapFrom(y => y.Logo.Replace(" ", "%20")));

            CreateMap<RestaurantDTO, Restaurant>()
                 .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
