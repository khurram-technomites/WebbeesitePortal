using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class ItemMapper : Profile
    {
        public ItemMapper()
        {
            CreateMap<ItemDTO, ItemViewModel>();
            CreateMap<ItemViewModel, ItemDTO>();
            CreateMap<ItemOptionDTO, ItemOptionViewModel>();
            CreateMap<ItemOptionViewModel, ItemOptionDTO>();
            CreateMap<ItemOptionValueDTO, ItemOptionValueViewModel>();
            CreateMap<ItemOptionValueViewModel, ItemOptionValueDTO>();
      
        }
    }
}
