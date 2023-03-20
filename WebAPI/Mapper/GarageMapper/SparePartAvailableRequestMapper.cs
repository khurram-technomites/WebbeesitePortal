using AutoMapper;
using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageMapper
{
    public class SparePartAvailableRequestMapper : Profile
    {
        public SparePartAvailableRequestMapper()
        {
            CreateMap<SparePartsAvailableRequestDTO, SparePartAvailableRequests>();
            CreateMap<SparePartAvailableRequests, SparePartsAvailableRequestDTO>();
        }
    }
}
