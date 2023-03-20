using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantDeliveryStaffMapper : Profile
    {
        public RestaurantDeliveryStaffMapper()
        {
            CreateMap<RestaurantDeliveryStaffViewModel, RestaurantDeliveryStaffDTO>();
            CreateMap<RestaurantDeliveryStaffDTO, RestaurantDeliveryStaffViewModel>();
        }
    }
}
