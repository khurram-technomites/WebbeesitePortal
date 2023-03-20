using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartTeamManagementMapper : Profile
    {
        public SparePartTeamManagementMapper()
        {
            CreateMap<SparePartTeamManagementDTO, SparePartTeamManagementViewModel>();
            CreateMap<SparePartTeamManagementViewModel, SparePartTeamManagementDTO>();
        }
    }
}
