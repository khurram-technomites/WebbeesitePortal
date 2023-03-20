using AutoMapper;
using HelperClasses.DTOs.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.PartnerMapper
{
    public class MenuPartnerMapper : Profile
    {
        public MenuPartnerMapper()
        {
            CreateMap<Menu, MenuPartnerDTO>();

            CreateMap<Category, MenuCategoryPartnerDTO>();

            CreateMap<MenuItem, MenuCategoryItemDTO>()
                .ForMember(x => x.Image, x => x.MapFrom(y => y.Item.Image));
            CreateMap<MenuCategoryItemDTO, MenuItem>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));

            CreateMap<MenuItemOption, MenuCategoryItemOptionsDTO>();
            CreateMap<MenuCategoryItemOptionsDTO, MenuItemOption>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));

            CreateMap<MenuItemOptionValue, MenuCategoryItemOptionValuesDTO>();
            CreateMap<MenuCategoryItemOptionValuesDTO, MenuItemOptionValue>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
