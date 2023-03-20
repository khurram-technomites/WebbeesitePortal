using AutoMapper;
using HelperClasses.DTOs.RestaurantDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantDashboardMapper : Profile
    {
        public RestaurantDashboardMapper()
        {
            CreateMap<RestaurantDashboardStatsDTO, RestaurantDashboardViewModel>();
            CreateMap<RestaurantDashboardViewModel, RestaurantDashboardStatsDTO>();
        }
    }
}
