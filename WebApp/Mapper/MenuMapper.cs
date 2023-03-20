using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Menu;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class MenuMapper : Profile
    {
        public MenuMapper()
        {
            CreateMap<MenuDTO, MenuViewModel>();
            CreateMap<MenuViewModel, MenuDTO>();
            CreateMap<MenuItemDTO, MenuItemViewModel>();
            CreateMap<MenuItemViewModel, MenuItemDTO>();
            CreateMap<MenuDTO, DuplicateMenuViewModel>();
            CreateMap<DuplicateMenuViewModel, MenuDTO>();
            CreateMap<MenuItemDTO, DuplicateMenuItemViewModel>();
            CreateMap<DuplicateMenuItemViewModel, MenuItemDTO>();
            CreateMap<MenuItemByMenuDTO, MenuItemByMenuViewModel>()
                .ForMember(x=>x.MenuItems , x => x.MapFrom(y => y.MenuItems));
            CreateMap<MenuItemByCategoryViewModel, MenuItemByCategoryDTO>();
            CreateMap<MenuItemByCategoryDTO, MenuItemByCategoryViewModel>();
            CreateMap<MenuItemByMenuViewModel, MenuItemByMenuDTO>();
            CreateMap<MenuItemOptionDTO, DuplicateMenuItemOptionViewModel>();
            CreateMap<DuplicateMenuItemOptionViewModel, MenuItemOptionDTO>();
            CreateMap<MenuItemOptionValueDTO, DuplicateMenuItemOptionValueViewModel>();
            CreateMap<DuplicateMenuItemOptionValueViewModel, MenuItemOptionValueDTO>();
            CreateMap<MenuItemOptionDTO, MenuItemOptionViewModel>();
            CreateMap<MenuItemOptionViewModel, MenuItemOptionDTO>();
            CreateMap<MenuItemOptionValueDTO, MenuItemOptionValueViewModel>();
            CreateMap<MenuItemOptionValueViewModel, MenuItemOptionValueDTO>();
            CreateMap<ItemViewModel, MenuItemDTO>()
            .ForMember(x => x.ItemId, x => x.MapFrom(y => y.Id))
            .ForMember(x => x.MenuItemOptions, x => x.MapFrom(y => y.ItemOptions));
            CreateMap<ItemOptionViewModel, MenuItemOptionViewModel>()
                .ForMember(x => x.MenuItemOptionValues, x => x.MapFrom(y => y.ItemOptionValues));
            CreateMap<ItemOptionValueViewModel, MenuItemOptionValueViewModel>();
  

        }
    }
}
