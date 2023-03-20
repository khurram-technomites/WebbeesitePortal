using AutoMapper;
using HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageAndSPWebsiteResponsesMapper.WebsiteResponses
{
    public class WebsiteTeamResponseDTOMapper : Profile
    {
        public WebsiteTeamResponseDTOMapper()
        {
            CreateMap<GarageTeamManagement, WebsiteTeamResponseDTO>();
            CreateMap<WebsiteTeamResponseDTO, GarageTeamManagement>();

            CreateMap<SparePartTeamManagement, WebsiteTeamResponseDTO>();
            CreateMap<WebsiteTeamResponseDTO, SparePartTeamManagement>();
        }
    }
}
