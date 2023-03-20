using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantContentManagementMapper : Profile
    {
        public RestaurantContentManagementMapper()
        {
            CreateMap<RestaurantContentManagementDTO, RestaurantContentManagementViewModel>();
            CreateMap<RestaurantContentManagementViewModel, RestaurantContentManagementDTO>();
        }
    }
}
