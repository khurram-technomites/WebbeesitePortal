using AutoMapper;
using HelperClasses.DTOs.RestaurantCashierStaff;
using WebApp.ViewModels.Restaurant;

namespace WebApp.Mapper
{
    public class RestaurantCashierStaffMapper : Profile
    {
        public RestaurantCashierStaffMapper()
        {
            CreateMap<RestaurantCashierStaffViewModel, RestaurantCashierStaffDTO>();
            CreateMap<RestaurantCashierStaffDTO, RestaurantCashierStaffViewModel>();
        }
    }
}
