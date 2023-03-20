using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantManagerMapper:Profile
    {
        public RestaurantManagerMapper()
        {
            CreateMap<RestaurantManagerViewModel, RestaurantManagerDTO>();
            CreateMap<RestaurantManagerDTO, RestaurantManagerViewModel>();
        }
    }
}
