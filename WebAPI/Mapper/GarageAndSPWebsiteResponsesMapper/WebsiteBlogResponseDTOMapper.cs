using AutoMapper;
using HelperClasses.DTOs.GarageAndSPWebsiteResponses.WebsiteResponses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageAndSPWebsiteResponsesMapper.WebsiteResponses
{
    public class SparePartBlogResponseDTOMapper : Profile
    {
        public SparePartBlogResponseDTOMapper()
        {
            CreateMap<GarageBlog, WebsiteBlogResponseDTO>()
                .ForMember(x => x.CreationDate, x => x.MapFrom(y => y.CreationDate.ToString("D")));

            CreateMap<SparePartBlog, WebsiteBlogResponseDTO>()
                .ForMember(x => x.CreationDate, x => x.MapFrom(y => y.CreationDate.ToString("D")));
        }
    }
}
