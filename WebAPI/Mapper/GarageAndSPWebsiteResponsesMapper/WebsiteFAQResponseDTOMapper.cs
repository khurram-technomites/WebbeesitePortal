using AutoMapper;
using HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageAndSPWebsiteResponsesMapper.WebsiteResponses
{
    public class SparePartFAQResponseDTOMapper : Profile
    {
        public SparePartFAQResponseDTOMapper()
        {
            CreateMap<GarageFAQ, WebsiteFAQResponseDTO>();
            CreateMap<SparePartFAQ, WebsiteFAQResponseDTO>();
        }
    }
}
