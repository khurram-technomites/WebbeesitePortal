using AutoMapper;
using HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageAndSPWebsiteResponsesMapper.WebsiteResponses
{
    public class WebsiteTestimonialsResponseDTOMapper : Profile
    {
        public WebsiteTestimonialsResponseDTOMapper()
        {
            CreateMap<GarageTestimonials, WebsiteTestimonialsResponseDTO>();
            CreateMap<SparePartTestimonial, WebsiteTestimonialsResponseDTO>();
        }
    }
}
