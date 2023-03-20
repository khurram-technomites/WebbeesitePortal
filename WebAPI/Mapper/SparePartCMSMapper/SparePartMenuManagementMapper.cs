using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartMenuManagementMapper : Profile
    {
        public SparePartMenuManagementMapper()
        {
            CreateMap<SparePartMenuManagement, SparePartMenuManagementDTO>();
            CreateMap<SparePartMenuManagementDTO, SparePartMenuManagement>();
        }
    }
}
