using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageExpertiseManagementMapper:Profile
    {
        public GarageExpertiseManagementMapper()
        {
            CreateMap<GarageExpertiseManagementDTO, GarageExpertiseManagement>();
            CreateMap<GarageExpertiseManagement, GarageExpertiseManagementDTO>();
        }
    }
}
