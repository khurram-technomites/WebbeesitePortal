using AutoMapper;
using HelperClasses.DTOs;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class CustomerCouponMapper : Profile
    {
        public CustomerCouponMapper()
        {
            CreateMap<CustomerCouponDTO, CustomerCouponViewModel>();
            CreateMap<CustomerCouponViewModel, CustomerCouponDTO>();
        }
    }
}
