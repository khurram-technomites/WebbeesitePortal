using AutoMapper;
using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageFAQViewMapper : Profile
    {
        public GarageFAQViewMapper()
        {
            CreateMap<GarageFAQDTO, GarageFAQViewModel>();
            CreateMap<GarageFAQViewModel, GarageFAQDTO>();
        }
    }
}
