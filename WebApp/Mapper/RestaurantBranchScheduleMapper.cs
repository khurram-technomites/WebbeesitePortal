using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using System;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantBranchScheduleMapper : Profile
    {
        public RestaurantBranchScheduleMapper()
        {
            CreateMap<RestaurantBranchScheduleDTO, RestaurantBranchScheduleViewModel>()
            .ForMember(x => x.FormattedOpeningTime, x => x.MapFrom(y => (DateTime.Today + y.OpeningTime).ToShortTimeString()))
            .ForMember(x => x.FormattedClosingTime, x => x.MapFrom(y => (DateTime.Today + y.ClosingTime).ToShortTimeString()));
            CreateMap<RestaurantBranchScheduleViewModel, RestaurantBranchScheduleDTO>();
        }
    }
}
