using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageAppointmentManagementMapper : Profile
    {
        public GarageAppointmentManagementMapper()
        {
            CreateMap<GarageAppointmentManagementDTO, GarageAppointmentManagement>();
            CreateMap<GarageAppointmentManagement, GarageAppointmentManagementDTO>();
        }
    }
}
