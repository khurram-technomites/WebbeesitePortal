using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageCustomerAppointmentMapper:Profile
    {
        public GarageCustomerAppointmentMapper()
        {
            CreateMap<GarageCustomerAppointment, GarageCustomerAppointmentDTO>();
            CreateMap<GarageCustomerAppointmentDTO, GarageCustomerAppointment>();
        }
    }
}
