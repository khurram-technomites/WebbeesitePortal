using AutoMapper;
using HelperClasses.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class BlogCategoryDTOMapper : Profile
    {
        public BlogCategoryDTOMapper()
        {
            CreateMap<BlogCategory, BlogCategoryDTO>();
            CreateMap<BlogCategoryDTO, BlogCategory>();
        }
    }
}
