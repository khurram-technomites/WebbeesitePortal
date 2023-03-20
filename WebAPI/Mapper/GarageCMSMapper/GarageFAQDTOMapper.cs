using AutoMapper;
using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageFAQDTOMapper : Profile
    {
        public GarageFAQDTOMapper()
        {
            CreateMap<GarageFAQ, GarageFAQDTO>();
            CreateMap<GarageFAQDTO, GarageFAQ>();
        }
    }
}
