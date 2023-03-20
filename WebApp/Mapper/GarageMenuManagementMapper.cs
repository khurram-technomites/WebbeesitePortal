using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageMenuManagementMapper:Profile
    {
        public GarageMenuManagementMapper()
        {
            CreateMap<GarageMenuManagementViewModel, GarageMenuManagementDTO>();
            CreateMap<GarageMenuManagementDTO, GarageMenuManagementViewModel>();
        }
    }
}
