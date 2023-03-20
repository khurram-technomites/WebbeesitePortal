using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartsDealerMapper
{
    public class SparePartsDealerMapper : Profile
    {
        public SparePartsDealerMapper()
        {
            CreateMap<SparePartsDealerDTO, SparePartsDealer>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
            CreateMap<SparePartsDealer, SparePartsDealerDTO>();
        }
    }
}
