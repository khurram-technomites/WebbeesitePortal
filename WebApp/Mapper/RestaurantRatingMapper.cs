using AutoMapper;
using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Admin.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantRatingMapper : Profile
    {
        public RestaurantRatingMapper()
        {
            CreateMap<RestaurantRatingDTO, RestaurantRatingViewModel>();
            CreateMap<RestaurantRatingViewModel, RestaurantRatingDTO>();
        }
    }
}
