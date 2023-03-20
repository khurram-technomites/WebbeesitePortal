using AutoMapper;
using HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageAndSPWebsiteResponsesMapper.WebsiteResponses
{
    public class SparePartExpertiseManagementDTOMapper : Profile
    {
        public SparePartExpertiseManagementDTOMapper()
        {
            CreateMap<GarageExpertiseManagement, WebsiteExpertiseResponseDTO>()
                .ForMember(x => x.Experties, x => x.MapFrom(y => y.GarageExpertise.Select(x => x.Expertise.Title)));

            CreateMap<SparePartExpertiseManagement, WebsiteExpertiseResponseDTO>()
                .ForMember(x => x.Experties, x => x.MapFrom(y => y.SparePartExpertise.Select(x => x.Expertise.Title)));
        }
    }
}
