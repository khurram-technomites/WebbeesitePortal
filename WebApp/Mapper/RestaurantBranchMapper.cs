using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantBranchMapper : Profile
    {
        public RestaurantBranchMapper()
        {
            CreateMap<RestaurantBranchViewModel, RestaurantBranchDTO>();
            CreateMap<RestaurantBranchDTO, RestaurantBranchViewModel>();
        }
    }
}
