using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantBalanceSheetMapper : Profile
    {
        public RestaurantBalanceSheetMapper()
        {
            CreateMap<RestaurantBalanceSheetViewModel, RestaurantBalanceSheetLogsDTO>();
            CreateMap<RestaurantBalanceSheetLogsDTO, RestaurantBalanceSheetViewModel>();
        }
    }
}
