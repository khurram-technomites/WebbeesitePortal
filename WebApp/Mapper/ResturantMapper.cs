using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class ResturantMapper : Profile
    {
        public ResturantMapper()
        {
            CreateMap<RestaurantDTO, RestaurantViewModel>();
            CreateMap<RestaurantViewModel, RestaurantDTO>();
        }
    }
}
