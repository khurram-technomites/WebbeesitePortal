using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageServiceManagementMapper:Profile
    {
        public GarageServiceManagementMapper()
        {
            CreateMap<GarageServiceManagement, GarageServiceManagementDTO>();
            CreateMap<GarageServiceManagementDTO, GarageServiceManagement>();
        }
    }
}
