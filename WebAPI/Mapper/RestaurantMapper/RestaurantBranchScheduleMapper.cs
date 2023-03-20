using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantBranchScheduleMapper : Profile
    {
        public RestaurantBranchScheduleMapper()
        {
            CreateMap<RestaurantBranchSchedule, RestaurantBranchScheduleDTO>()
                .ForMember(x => x.FormattedOpeningTime, x => x.MapFrom(y => (DateTime.Today + y.OpeningTime).ToShortTimeString()))
                .ForMember(x => x.FormattedClosingTime, x => x.MapFrom(y => (DateTime.Today + y.ClosingTime).ToShortTimeString()));

            CreateMap<RestaurantBranchScheduleDTO, RestaurantBranchSchedule>();
        }
    }
}
