using AutoMapper;
using HelperClasses.DTOs.RestaurantServiceStaff;
using HelperClasses.DTOs.ServiceStaff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class RestaurantServiceStaffMapper : Profile
    {
        public RestaurantServiceStaffMapper()
        {
            CreateMap<RestaurantServiceStaffViewModel, RestaurantServiceStaffDTO>();
            CreateMap<RestaurantServiceStaffDTO, RestaurantServiceStaffViewModel>();
            CreateMap<RestaurantServiceStaff, RestaurantServiceStaffDTO>();
            CreateMap<RestaurantServiceStaffDTO, RestaurantServiceStaff>();
            CreateMap<RestaurantRegisterServiceStaffViewModel, RestaurantServiceStaffRegisterDTO>();
            CreateMap<RestaurantServiceStaffRegisterDTO, RestaurantRegisterServiceStaffViewModel>();
        }
    }
}
