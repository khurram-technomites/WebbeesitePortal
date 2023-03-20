﻿using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartBlogMapper : Profile
    {
        public SparePartBlogMapper()
        {
            CreateMap<SparePartBlog, SparePartBlogDTO>();
            CreateMap<SparePartBlogDTO, SparePartBlog>();

        }
    }
}
