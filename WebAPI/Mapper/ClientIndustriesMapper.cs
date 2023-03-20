using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class ClientIndustriesMapper : Profile
    {
        public ClientIndustriesMapper()
        {
            CreateMap<ClientIndustries, ClientIndustriesDTO>();
            CreateMap<ClientIndustriesDTO, ClientIndustries>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
