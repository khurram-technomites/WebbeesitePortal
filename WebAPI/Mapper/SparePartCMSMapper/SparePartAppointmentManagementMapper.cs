using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartAppointmentManagementMapper : Profile
    {
        public SparePartAppointmentManagementMapper()
        {
            CreateMap<SparePartAppointmentManagementDTO, SparePartAppointmentManagement>();
            CreateMap<SparePartAppointmentManagement, SparePartAppointmentManagementDTO>();
        }
    }
}
