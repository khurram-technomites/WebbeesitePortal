using AutoMapper;
using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageMapper
{
    public class GarageImagesDTOMapper : Profile
    {
        public GarageImagesDTOMapper()
        {
            CreateMap<GarageImage, GarageImageDTO>();
            CreateMap<GarageImageDTO, GarageImage>();
        }
    }
}
