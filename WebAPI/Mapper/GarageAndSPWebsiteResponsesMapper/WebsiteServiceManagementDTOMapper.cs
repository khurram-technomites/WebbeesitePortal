using AutoMapper;
using HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageAndSPWebsiteResponsesMapper.WebsiteResponses
{
    public class WebsiteServiceManagementDTOMapper : Profile
    {
        public WebsiteServiceManagementDTOMapper()
        {
            CreateMap<WebsiteServiceManagementResponseDTO, GarageServiceManagement>();
            CreateMap<GarageServiceManagement, WebsiteServiceManagementResponseDTO>();

            CreateMap<WebsiteServiceManagementResponseDTO, SparePartServiceManagement>();
            CreateMap<SparePartServiceManagement, WebsiteServiceManagementResponseDTO>();
        }
    }
}
