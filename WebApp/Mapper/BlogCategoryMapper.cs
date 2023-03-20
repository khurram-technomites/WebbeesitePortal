using AutoMapper;
using HelperClasses.DTOs.Blog;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class BlogCategoryMapper:Profile
    {
        public BlogCategoryMapper()
        {
            CreateMap<BlogCategoryDTO, BlogCategoryViewModel>();
            CreateMap<BlogCategoryViewModel, BlogCategoryDTO>();
           
        }
    }
}
