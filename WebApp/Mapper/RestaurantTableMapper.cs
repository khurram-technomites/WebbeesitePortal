using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantTableMapper:Profile
    {
        public RestaurantTableMapper()
        {
            CreateMap<RestaurantTableViewModel, RestaurantTableDTO>();
            CreateMap<RestaurantTableDTO, RestaurantTableViewModel>();
        }
    }
}
