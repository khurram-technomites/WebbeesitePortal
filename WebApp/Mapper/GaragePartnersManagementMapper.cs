using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GaragePartnersManagementMapper:Profile
    {
        public GaragePartnersManagementMapper()
        {
            CreateMap<GaragePartnersManagementViewModel, GaragePartnersManagementDTO>();
            CreateMap<GaragePartnersManagementDTO, GaragePartnersManagementViewModel>();
        }
    }
}
