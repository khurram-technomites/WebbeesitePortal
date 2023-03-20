using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class FCMUserSessionMapper : Profile
    {
        public FCMUserSessionMapper()
        {
            CreateMap<FCMUserSession, FCMUserSessionDTO>();
            CreateMap<FCMUserSessionDTO, FCMUserSession>();
        }
    }
}
