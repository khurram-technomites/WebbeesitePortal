using AutoMapper;
using HelperClasses.Classes;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantCashierStaff;
using System;
using System.Linq;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantCashierStaffMapper : Profile
    {
        public RestaurantCashierStaffMapper()
        {
            CreateMap<RestaurantCashierStaff, RestaurantCashierStaffDTO>()
                .ForMember(x => x.DeliveryCharges, x => x.MapFrom(y => y.RestaurantBranch.DeliveryCharges))
                .ForMember(x => x.TaxPercentage, x => x.MapFrom(y => y.Restaurant.TaxPercent))
               .ForMember(x => x.RestaurantName, x => x.MapFrom(y => y.Restaurant.NameAsPerTradeLicense))
               .ForMember(des => des.RestaurantWebsite, opt => opt.MapFrom(src => src.Restaurant != null ? src.Restaurant.Website : "-"))
               .ForMember(des => des.RestaurantVATRegistrationNumber, opt => opt.MapFrom(src => src.Restaurant != null ? src.Restaurant.VATRegistrationNumber : "-"))
               .ForMember(des => des.RestaurantBranchName, opt => opt.MapFrom(src => src.RestaurantBranch != null ? src.RestaurantBranch.NameAsPerTradeLicense : "-"))
               .ForMember(des => des.RestaurantBranchContact, opt => opt.MapFrom(src => src.RestaurantBranch != null ? src.RestaurantBranch.OrderingPhoneNumber : "-"))
               .ForMember(x => x.IsShiftClose, x => x.MapFrom(y =>
               y.RestaurantBalanceSheets.Count > 0 ? (y.RestaurantBalanceSheets.LastOrDefault().Status != Enum.GetName(typeof(BalanceSheetStatus), BalanceSheetStatus.Opened))
               : true))
               .ForMember(x => x.RestaurantBalanceSheet, x => x.MapFrom(y => y.RestaurantBalanceSheets.LastOrDefault()))
               .ForMember(x => x.IsClose, x => x.MapFrom(y => y.RestaurantBranch.IsClose));

            CreateMap<RestaurantCashierStaffDTO, RestaurantCashierStaff>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
