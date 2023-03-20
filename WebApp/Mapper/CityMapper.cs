using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class CityMapper : Profile
    {
        public CityMapper()
        {
            CreateMap<CityDTO, CityViewModel>();
            CreateMap<CityViewModel, CityDTO>();
        }
    }
}
