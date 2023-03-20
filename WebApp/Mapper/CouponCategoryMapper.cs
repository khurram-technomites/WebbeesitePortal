using AutoMapper;
using HelperClasses.DTOs;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class CouponCategoryMapper : Profile
    {
        public CouponCategoryMapper()
        {
            CreateMap<CouponCategoryDTO, CouponCategoryViewModel>();
            CreateMap<CouponCategoryViewModel, CouponCategoryDTO>();
        }
    }
}
