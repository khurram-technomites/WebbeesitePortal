using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartBlogMapper : Profile
    {
        public SparePartBlogMapper()
        {
            CreateMap<SparePartBlogDTO, SparePartBlogViewModel>();
            CreateMap<SparePartBlogViewModel, SparePartBlogDTO>();
        }
    }
}
