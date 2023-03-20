using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Supplier;
using WebAPI.Models;

namespace WebAPI.Mapper.SupplierMapper
{
    public class SupplierCouponMapper : Profile
    {
        public SupplierCouponMapper()
        {
            CreateMap<SupplierCoupon, SupplierCouponDTO>();
            CreateMap<SupplierCouponDTO, SupplierCoupon>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
            CreateMap<SupplierCouponCategory, SupplierCouponCategoryDTO>();
            CreateMap<SupplierCouponCategoryDTO, SupplierCouponCategory>();
            CreateMap<SupplierCouponRedemption, SupplierCouponRedemptionDTO>();
            CreateMap<SupplierCouponRedemptionDTO, SupplierCouponRedemption>();
        }
    }
}
