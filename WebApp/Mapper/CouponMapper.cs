using AutoMapper;
using HelperClasses.DTOs;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class CouponMapper : Profile
    {
        public CouponMapper()
        {
            CreateMap<CouponViewModel, CouponDTO>();
            CreateMap<CouponDTO, CouponViewModel>();
        }
    }
}
