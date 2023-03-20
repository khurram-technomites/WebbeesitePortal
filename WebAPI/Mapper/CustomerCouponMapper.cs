using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class CustomerCouponMapper : Profile
    {
        public CustomerCouponMapper()
        {
            CreateMap<CustomerCoupon, CustomerCouponDTO>();
            CreateMap<CustomerCouponDTO, CustomerCoupon>();
        }
    }
}
