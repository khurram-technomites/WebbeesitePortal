using AutoMapper;
using HelperClasses.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class BlogMapper : Profile
    {
        public BlogMapper()
        {
            CreateMap<Blogs, BlogDTO>()
                .ForMember(x=>x.FormattedDate, x=>x.MapFrom(y=>y.CreationDate.ToString("dd MMM yyyy")));
            CreateMap<BlogDTO, Blogs>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));

            CreateMap<Blogs, BlogCardResponse>()
                .ForMember(x => x.CreatedDate, x => x.MapFrom(y => y.CreationDate.ToString("dd MMM yyyy")));
        }
    }
}
