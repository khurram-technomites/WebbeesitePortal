using AutoMapper;
using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartsDealerMapper
{
    public class SparePartsDealerScheduleDTOMapper : Profile
    {
        public SparePartsDealerScheduleDTOMapper()
        {
            CreateMap<DealerSchedule, SparePartsDealerScheduleDTO>()
                .ForMember(x => x.FormattedOpeningTime, x => x.MapFrom(y => (DateTime.Today + y.OpeningTime).ToShortTimeString()))
                .ForMember(x => x.FormattedClosingTime, x => x.MapFrom(y => (DateTime.Today + y.ClosingTime).ToShortTimeString()));
            CreateMap<SparePartsDealerScheduleDTO, DealerSchedule>();
        }
    }
}
