using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantUserLogManagementMapper : Profile
    {
        public RestaurantUserLogManagementMapper()
        {
            CreateMap<RestaurantUserLogManagementViewModel, RestaurantUserLogManagementDTO>();
            CreateMap<RestaurantUserLogManagementDTO, RestaurantUserLogManagementViewModel>();
        }
    }
}
