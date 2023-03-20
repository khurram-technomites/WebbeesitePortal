using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageProjectImageMapper : Profile
    {
        public GarageProjectImageMapper()
        {
            CreateMap<GarageProjectImages, GarageProjectImageDTO>();
            CreateMap<GarageProjectImageDTO, GarageProjectImages>();
        }
    }
}
