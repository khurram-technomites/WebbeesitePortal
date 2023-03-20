using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using HelperClasses.DTOs.RestaurantKitchenManager;
using WebAPI.Models;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantKitchenManagerMapper:Profile
    {
        public RestaurantKitchenManagerMapper()
        {
            CreateMap<RestaurantKitchenManagerViewModel, RestaurantKitchenManagerDTO>();
            CreateMap<RestaurantKitchenManagerDTO, RestaurantKitchenManagerViewModel>();
        }
    }
}
