﻿using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class CityMapper : Profile
    {
        public CityMapper()
        {
            CreateMap<City, CityDTO>();
            CreateMap<CityDTO, City>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}