using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartContentManagementMapper : Profile
    {
        public SparePartContentManagementMapper()
        {
            CreateMap<SparePartContentManagement, SparePartContentManagementDTO>();
            CreateMap<SparePartContentManagementDTO, SparePartContentManagement>();
        }
    }
}
