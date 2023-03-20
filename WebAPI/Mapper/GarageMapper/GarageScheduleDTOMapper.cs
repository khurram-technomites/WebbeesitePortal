using AutoMapper;
using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageMapper
{
    public class GarageScheduleDTOMapper : Profile
    {
        public GarageScheduleDTOMapper()
        {
            CreateMap<GarageSchedule, GarageScheduleDTO>()
                .ForMember(x => x.FormattedOpeningTime, x => x.MapFrom(y => (DateTime.Today + y.OpeningTime).ToShortTimeString()))
                .ForMember(x => x.FormattedClosingTime, x => x.MapFrom(y => (DateTime.Today + y.ClosingTime).ToShortTimeString()));
            CreateMap<GarageScheduleDTO, GarageSchedule>();
        }
    }
}
