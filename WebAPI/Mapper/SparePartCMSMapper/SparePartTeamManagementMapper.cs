using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartTeamManagementMapper : Profile
    {
        public SparePartTeamManagementMapper()
        {
            CreateMap<SparePartTeamManagementDTO, SparePartTeamManagement>();
            CreateMap<SparePartTeamManagement, SparePartTeamManagementDTO>();
        }
    }
}
