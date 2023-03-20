using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageProjectViewMapper : Profile
    {
        public GarageProjectViewMapper()
        {
            CreateMap<GarageProjectDTO, GarageProjectViewModel>();
            CreateMap<GarageProjectViewModel, GarageProjectDTO>();
        }
    }
}
