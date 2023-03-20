using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class CustomerAddressMapper : Profile
    {
        public CustomerAddressMapper()
        {
            CreateMap<CustomerAddress, CustomerAddressDTO>();
            CreateMap<CustomerAddressDTO, CustomerAddress>();
        }
    }
}
