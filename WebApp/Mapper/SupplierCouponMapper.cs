using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SupplierCouponMapper : Profile
    {
        public SupplierCouponMapper()
        {
            CreateMap<SupplierCouponDTO, SupplierCouponViewModel>()
                 .ForMember(x => x.RemainingDays , x => x.MapFrom(y => (y.Expiry.Value.Date - DateTime.UtcNow.ToDubaiDateTime().Date).Days < 0 ? 0 : (y.Expiry.Value.Date - DateTime.UtcNow.ToDubaiDateTime().Date).Days)); 
            CreateMap<SupplierCouponViewModel, SupplierCouponDTO>();

            CreateMap<RestaurantCouponDTO, RestaurantCouponViewModel>();
            CreateMap<RestaurantCouponViewModel, RestaurantCouponDTO>();


            CreateMap<SupplierCouponRedemptionDTO, SupplierCouponRedemptionViewModel>();
            CreateMap<SupplierCouponRedemptionViewModel, SupplierCouponRedemptionDTO>();

        }
    }
}
