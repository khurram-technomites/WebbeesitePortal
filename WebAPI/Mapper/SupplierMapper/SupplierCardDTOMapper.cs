using AutoMapper;
using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.SupplierMapper
{
    public class SupplierCardDTOMapper : Profile
    {
        public SupplierCardDTOMapper()
        {
            CreateMap<Supplier, SupplierCardDTO>();
        }
    }
}
