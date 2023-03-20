using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageTeamManagementMapper:Profile
    {
        public GarageTeamManagementMapper()
        {
            CreateMap<GarageTeamManagementDTO, GarageTeamManagement>();
            CreateMap<GarageTeamManagement, GarageTeamManagementDTO>();
        }
    }
}
