using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GaragePartnersManagementMapper:Profile
    {
        public GaragePartnersManagementMapper()
        {
            CreateMap<GaragePartnersManagement, GaragePartnersManagementDTO>();
            CreateMap<GaragePartnersManagementDTO, GaragePartnersManagement>();
        }
    }
}
