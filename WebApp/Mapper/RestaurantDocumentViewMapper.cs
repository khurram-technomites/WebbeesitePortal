using AutoMapper;
using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantDocumentViewMapper : Profile
    {
        public RestaurantDocumentViewMapper()
        {
            CreateMap<RestaurantDocumentDTO, RestaurantDocumentViewModel>();
            CreateMap<RestaurantDocumentViewModel, RestaurantDocumentDTO>();
        }
    }
}
