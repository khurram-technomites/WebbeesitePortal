using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class ItemOptionValueMapper : Profile
    {
        public ItemOptionValueMapper()
        {
            CreateMap<ItemOptionValue, ItemOptionValueDTO>();
            CreateMap<ItemOptionValueDTO, ItemOptionValue>();
        }
    }
}
