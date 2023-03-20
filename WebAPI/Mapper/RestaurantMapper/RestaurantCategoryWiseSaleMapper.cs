using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;

namespace WebAPI.Mapper.RestaurantMapper
{
    public class RestaurantCategoryWiseSaleMapper:Profile
    {
        public RestaurantCategoryWiseSaleMapper()
        {
            CreateMap<RestaurantCategoryWiseSale, RestaurantCategoryWiseSaleDTO>()
                .ForMember(des => des.OrderDetailId, opt => opt.Condition(src => src.OrderDetail !=null))
                .ForMember(des => des.RestaurantBalanceSheetId, opt => opt.Condition(src => src.RestaurantBalanceSheet.Id != 0))
                .ForMember(des => des.CategoryId, opt => opt.Condition(src => src.Category.Id != 0))
                .ForMember(des => des.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
                ;
            CreateMap<RestaurantCategoryWiseSaleDTO, RestaurantCategoryWiseSale>();

        }
    }
}
