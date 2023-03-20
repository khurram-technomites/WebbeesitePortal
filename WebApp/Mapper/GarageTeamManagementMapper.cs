using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageTeamManagementMapper:Profile
    {
        public GarageTeamManagementMapper()
        {
            CreateMap<GarageTeamManagementViewModel, GarageTeamManagementDTO>();
            CreateMap<GarageTeamManagementDTO, GarageTeamManagementViewModel>();
        }
    }
}
