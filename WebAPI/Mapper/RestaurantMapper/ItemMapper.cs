using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class ItemMapper : Profile
    {
        public ItemMapper()
        {
            CreateMap<Item, ItemDTO>();
            CreateMap<ItemDTO, Item>();
            CreateMap<ItemOption, ItemOptionDTO>();
            CreateMap<ItemOptionDTO, ItemOption>();
            CreateMap<ItemOptionValue, ItemOptionValueDTO>();
            CreateMap<ItemOptionValueDTO, ItemOptionValue>();
        }
    }
}
