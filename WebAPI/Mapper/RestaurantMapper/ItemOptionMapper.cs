using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class ItemOptionMapper : Profile
    {
        public ItemOptionMapper()
        {
            CreateMap<ItemOption, ItemOptionDTO>();
            CreateMap<ItemOptionDTO, ItemOption>();
        }
    }
}
