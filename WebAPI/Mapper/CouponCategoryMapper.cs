using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class CouponCategoryMapper : Profile
    {
        public CouponCategoryMapper()
        {
            CreateMap<CouponCategory, CouponCategoryDTO>();
            CreateMap<CouponCategoryDTO, CouponCategory>();
        }
    }
}
