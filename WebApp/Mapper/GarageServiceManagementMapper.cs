using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageServiceManagementMapper:Profile
    {
        public GarageServiceManagementMapper()
        {
            CreateMap<GarageServiceManagementViewModel, GarageServiceManagementDTO>();
            CreateMap<GarageServiceManagementDTO, GarageServiceManagementViewModel>();
        }
    }
}
