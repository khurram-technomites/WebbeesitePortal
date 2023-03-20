using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageMenuManagementMapper:Profile
    {
        public GarageMenuManagementMapper()
        {
            CreateMap<GarageMenuManagement, GarageMenuManagementDTO>();
            CreateMap<GarageMenuManagementDTO, GarageMenuManagement>();
        }
    }
}
