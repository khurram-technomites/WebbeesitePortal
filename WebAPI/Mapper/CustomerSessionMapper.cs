using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class CustomerSessionMapper : Profile
    {
        public CustomerSessionMapper()
        {
            CreateMap<CustomerSession, CustomerSessionDTO>();
            CreateMap<CustomerSessionDTO, CustomerSession>();
        }
    }
}
