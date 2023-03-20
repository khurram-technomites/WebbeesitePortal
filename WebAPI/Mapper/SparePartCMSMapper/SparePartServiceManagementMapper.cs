using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartServiceManagementMapper : Profile
    {
        public SparePartServiceManagementMapper()
        {
            CreateMap<SparePartServiceManagement, SparePartServiceManagementDTO>();
            CreateMap<SparePartServiceManagementDTO, SparePartServiceManagement>();
        }
    }
}
