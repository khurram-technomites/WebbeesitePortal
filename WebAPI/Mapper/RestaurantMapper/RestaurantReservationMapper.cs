using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantReservationMapper : Profile
    {
        public RestaurantReservationMapper()
        {
            CreateMap<RestaurantReservation,RestaurantReservationDTO>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
            ;
            CreateMap<RestaurantReservationDTO, RestaurantReservation>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
            ;
        }
    }
}
