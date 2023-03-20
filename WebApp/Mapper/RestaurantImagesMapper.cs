using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantImagesMapper : Profile
    {
        public RestaurantImagesMapper()
        {
            CreateMap<RestaurantImagesDTO, RestaurantImagesViewModel>();
            CreateMap<RestaurantImagesViewModel, RestaurantImagesDTO>();
        }
    }
}
