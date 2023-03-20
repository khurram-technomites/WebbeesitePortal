using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class BlogMapper : Profile
    {
        public BlogMapper()
        {
            CreateMap<BlogDTO, BlogViewModel>();
            CreateMap<BlogViewModel, BlogDTO>();
        }
        
    }
}
