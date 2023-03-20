using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantDeliveryStaffMapper : Profile
    {
        public RestaurantDeliveryStaffMapper()
        {
            CreateMap<RestaurantDeliveryStaff, RestaurantDeliveryStaffDTO>()
                .ForMember(des => des.BranchName, opt => opt.MapFrom(src => src.RestaurantBranch != null ? src.RestaurantBranch.NameAsPerTradeLicense : "-"))
                .ForMember(des => des.Logo, opt => opt.MapFrom(src => src.Logo != null ? src.Logo : "https://cdn.fougitodemo.com/Images/RestaurantDeliveryStaff/delivery-staff.jpg"));
            CreateMap<RestaurantDeliveryStaffDTO, RestaurantDeliveryStaff>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
