using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryViewModel, CategoryDTO>();
            CreateMap<CategoryDTO, CategoryViewModel>();
        }
    }
}
