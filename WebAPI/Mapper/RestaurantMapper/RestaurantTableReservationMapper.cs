using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantTableReservationMapper:Profile
    {
        public RestaurantTableReservationMapper()
        {
            CreateMap<RestaurantTableReservation, RestaurantTableReservationDTO>();
            CreateMap<RestaurantTableReservationDTO, RestaurantTableReservation>();
        }
    }
}
