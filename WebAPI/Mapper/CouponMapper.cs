using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class CouponMapper : Profile
    {
        public CouponMapper()
        {
            CreateMap<Coupon, CouponDTO>();
            CreateMap<CouponDTO, Coupon>();
        }
    }
}
