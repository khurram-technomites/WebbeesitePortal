using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartExpertiseManagementMapper : Profile
    {
        public SparePartExpertiseManagementMapper()
        {
            CreateMap<SparePartExpertiseManagement, SparePartExpertiseManagementDTO>();
            CreateMap<SparePartExpertiseManagementDTO, SparePartExpertiseManagement>();
        }
    }
}
