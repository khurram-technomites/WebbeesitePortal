using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantContentManagementMapper : Profile
    {
        public RestaurantContentManagementMapper()
        {
            CreateMap<RestaurantContentManagement, RestaurantContentManagementDTO>();
            CreateMap<RestaurantContentManagementDTO, RestaurantContentManagement>()
                .ForMember(x => x.PrivacyPolicy, x => x.MapFrom(y => y.PrivacyPolicy))
                .ForMember(x => x.DeliveryPolicy, x => x.MapFrom(y => y.DeliveryPolicy))
                .ForMember(x => x.ReturnPolicy, x => x.MapFrom(y => y.ReturnPolicy))
                .ForMember(x => x.TermsAndConditions, x => x.MapFrom(y => y.TermsAndConditions))
                .ForMember(x => x.AboutUs, x => x.MapFrom(y => y.AboutUs))
                .ForAllOtherMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
