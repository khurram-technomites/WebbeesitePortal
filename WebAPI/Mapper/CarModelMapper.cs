using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class CarModelMapper : Profile
    {
        public CarModelMapper()
        {
            CreateMap<CarModel, CarModelDTO>();
            CreateMap<CarModelDTO, CarModel>();
        }
    }
}
