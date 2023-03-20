using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageMapper
{
    public class GarageRepairSpecificationDTO : Profile
    {
        public GarageRepairSpecificationDTO()
        {
            CreateMap<GarageRepairSpecification, HelperClasses.DTOs.Garage.GarageRepairSpecificationDTO>()
                .ForMember(x => x.CarMakeName, x => x.MapFrom(y => y.CarMake.Name))
                .ForMember(x => x.CarMakeLogo, x => x.MapFrom(y => y.CarMake.Logo));
            CreateMap<HelperClasses.DTOs.Garage.GarageRepairSpecificationDTO, GarageRepairSpecification>();
        }
    }
}
