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
    public class RestaurantCustomerViewMapper : Profile
    {
        public RestaurantCustomerViewMapper()
        {
            CreateMap<RestaurantCustomerViewModel, RestaurantCustomerDTO>();
            CreateMap<RestaurantCustomerDTO, RestaurantCustomerViewModel>();
        }
    }
}
