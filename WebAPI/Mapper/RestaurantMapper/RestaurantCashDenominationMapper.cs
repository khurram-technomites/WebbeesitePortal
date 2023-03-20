using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantCashDenominationMapper:Profile
    {
        public RestaurantCashDenominationMapper()
        {
            CreateMap<RestaurantCashDenomination, RestaurantCashDenominationDTO>();
            CreateMap<RestaurantCashDenominationDTO, RestaurantCashDenomination>();
        }
    }
}
