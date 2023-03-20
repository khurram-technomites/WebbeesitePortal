using AutoMapper;
using HelperClasses.DTOs.RestaurantServiceStaff;
using HelperClasses.DTOs.ServiceStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;


namespace WebAPI.Mapper.RestaurantMapper
{

    public class RestaurantServiceStaffMapper : Profile
    {
        public RestaurantServiceStaffMapper()
        {
            CreateMap<RestaurantServiceStaff, RestaurantServiceStaffDTO>()
                .ForMember(x => x.RestaurantName, x => x.MapFrom(y => y.Restaurant.NameAsPerTradeLicense))
                .ForMember(x => x.IsClose, x => x.MapFrom(y => y.RestaurantBranch.IsClose))
                .ForMember(x => x.BranchName, x => x.MapFrom(y => y.RestaurantBranch != null ? y.RestaurantBranch.NameAsPerTradeLicense : "-"));

            CreateMap<RestaurantServiceStaffDTO, RestaurantServiceStaff>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
