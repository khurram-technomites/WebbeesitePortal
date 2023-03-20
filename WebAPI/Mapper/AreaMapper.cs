using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class AreaMapper : Profile
    {
        public AreaMapper()
        {
            CreateMap<Areas, AreaDTO>();
            CreateMap<AreaDTO, Areas>();
        }
    }
}
