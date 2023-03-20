using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Menu;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class MenuMapper : Profile
    {
        public MenuMapper()
        {
            CreateMap<MenuItem, ItemForMenuDTO>()
                //.ForMember(x => x.Id, x => x.MapFrom(y => y.Item.Id))
                //.ForMember(x => x.Name, x => x.MapFrom(y => y.Item.Name))
                //.ForMember(x => x.Description, x => x.MapFrom(y => y.Item.Description))
                //.ForMember(x => x.Price, x => x.MapFrom(y => y.Item.Price))
                .ForMember(x => x.Image, x => x.MapFrom(y => y.Item.Image))
                .ForMember(x => x.MenuItemOptions, x => x.MapFrom(y => y.MenuItemOptions));

            CreateMap<MenuItem, MenuItemByCategoryDTO>();

            CreateMap<MenuItemOption, ItemQuestions>()
                .ForMember(x => x.IsMain, x => x.MapFrom(y => y.IsPriceMain))
                .ForMember(x => x.MaxLimit, x => x.MapFrom(y => y.Maximum));
            CreateMap<MenuItemOptionValue, ItemQuestionOptions>();
            CreateMap<Menu, MenuDTO>()
                .ForMember(x => x.MenuItemCount, x => x.MapFrom(y => y.MenuItem.Count));
            CreateMap<MenuDTO, Menu>();
            
            CreateMap<MenuItemDTO, MenuItem>();
            CreateMap<MenuItem, MenuItemDTO>()
                .ForMember(x => x.RequiredCheck, x => x.MapFrom(y => y.MenuItemOptions.Any(x => x.IsRequired == true)))
                //.ForMember(x => x.Image, x => x.MapFrom(y => y.Item.Image));
                ;

            CreateMap<MenuItemOption, MenuItemOptionDTO>();
            CreateMap<MenuItemOptionDTO, MenuItemOption>();

            CreateMap<MenuItemOptionValue, MenuItemOptionValueDTO>();
            CreateMap<MenuItemOptionValueDTO, MenuItemOptionValue>();
        }
    }
}
