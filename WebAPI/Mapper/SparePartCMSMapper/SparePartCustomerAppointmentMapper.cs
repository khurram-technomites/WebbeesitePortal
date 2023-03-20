using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartCustomerAppointmentMapper : Profile
    {
        public SparePartCustomerAppointmentMapper()
        {
            CreateMap<SparePartCustomerAppointment, SparePartCustomerAppointmentDTO>();
            CreateMap<SparePartCustomerAppointmentDTO, SparePartCustomerAppointment>();
        }
    }
}
