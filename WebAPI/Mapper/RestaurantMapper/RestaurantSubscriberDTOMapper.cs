using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantSubscriberDTOMapper : Profile
    {
        public RestaurantSubscriberDTOMapper()
        {
            CreateMap<RestaurantSubscriber, RestaurantSubscriberDTO>();
            CreateMap<RestaurantSubscriberDTO, RestaurantSubscriber>();
        }
    }
}
