using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartPartnersManagementMapper : Profile
    {
        public SparePartPartnersManagementMapper()
        {
            CreateMap<SparePartPartnersManagement, SparePartPartnersManagementDTO>();
            CreateMap<SparePartPartnersManagementDTO, SparePartPartnersManagement>();
        }
    }
}
