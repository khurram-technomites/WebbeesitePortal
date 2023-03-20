using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantTransactionHistoryMapping : Profile
    {
        public RestaurantTransactionHistoryMapping()
        {
            CreateMap<RestaurantTransactionHistory, RestaurantTransactionHistoryDTO>();
            CreateMap<RestaurantTransactionHistoryDTO, RestaurantTransactionHistory>();
        }
    }
}
