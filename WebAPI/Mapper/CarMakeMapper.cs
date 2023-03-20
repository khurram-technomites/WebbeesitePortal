using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class CarMakeMapper : Profile
    {
        public CarMakeMapper()
        {
            CreateMap<CarMake, CarMakeDTO>();
            CreateMap<CarMakeDTO, CarMake>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
