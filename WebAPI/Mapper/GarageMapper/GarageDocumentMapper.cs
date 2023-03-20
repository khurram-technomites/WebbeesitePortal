using AutoMapper;
using HelperClasses.DTOs.Garage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageMapper
{
    public class GarageDocumentMapper : Profile
    {
        public GarageDocumentMapper()
        {
            CreateMap<GarageDocument, GarageDocumentDTO>();
            CreateMap<GarageDocumentDTO, GarageDocument>();
        }
    }
}
