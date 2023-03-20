using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class ClientTypesMapper : Profile
    {
        public ClientTypesMapper()
        {
            CreateMap<ClientTypes, ClientTypesDTO>();
            CreateMap<ClientTypesDTO, ClientTypes>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
