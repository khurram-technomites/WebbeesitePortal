using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantCouponMapper : Profile
    {
        public RestaurantCouponMapper()
        {
            CreateMap<RestaurantCoupon, RestaurantCouponDTO>();
            CreateMap<RestaurantCouponDTO, RestaurantCoupon>();
        }
    }
}
