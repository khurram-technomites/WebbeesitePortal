using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantProductWiseSaleMapper:Profile
    {
        public RestaurantProductWiseSaleMapper()
        {
            CreateMap<RestaurantProductWiseSale, RestaurantProductWiseSaleDTO>()
                .ForMember(des => des.OrderDetailId, opt => opt.Condition(src => src.OrderDetail != null))
                .ForMember(des => des.RestaurantBalanceSheetId, opt => opt.Condition(src => src.RestaurantBalanceSheet.Id != 0))
                .ForMember(des => des.MenuItemId, opt => opt.Condition(src => src.MenuItem.Id != 0))
                .ForMember(des => des.MenuItemName, opt => opt.MapFrom(src => src.MenuItem != null ? src.MenuItem.Name : null))
                ;
            CreateMap<RestaurantProductWiseSaleDTO, RestaurantProductWiseSale>();
        }
    }
}
