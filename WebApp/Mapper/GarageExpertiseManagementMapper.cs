using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageExpertiseManagementMapper:Profile
    {
        public GarageExpertiseManagementMapper()
        {
            CreateMap<GarageExpertiseManagementViewModel, GarageExpertiseManagementDTO>();
            CreateMap<GarageExpertiseManagementDTO, GarageExpertiseManagementViewModel>();
        }
    }
}
