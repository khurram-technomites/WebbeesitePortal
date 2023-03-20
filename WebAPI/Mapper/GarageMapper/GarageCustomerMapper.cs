using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageMapper
{
    public class GarageCustomerMapper : Profile
    {
        public GarageCustomerMapper()
        {
            CreateMap<GarageCustomerInvoiceDTO, GarageCustomerInvoice>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));

            CreateMap<GarageCustomerInvoice, GarageCustomerInvoiceDTO>();
        }
    }
}
