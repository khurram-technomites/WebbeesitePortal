using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageProjectImageViewMapper : Profile
    {
        public GarageProjectImageViewMapper()
        {
            CreateMap<GarageProjectImageDTO, GarageProjectImageViewModel>();
            CreateMap<GarageProjectImageViewModel, GarageProjectImageDTO>();
        }
    }
}