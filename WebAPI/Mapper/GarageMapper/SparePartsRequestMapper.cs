using AutoMapper;
using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageMapper
{
    public class SparePartsRequestMapper : Profile
    {
        public SparePartsRequestMapper()
        {
            CreateMap<SparePartRequestDTO, SparePartRequest>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
            CreateMap<SparePartRequest, SparePartRequestDTO>()
                .ForMember(x => x.OffersCount, x => x.MapFrom(y => y.SparePartRequestQuotes.Count));
        }
    }
}
