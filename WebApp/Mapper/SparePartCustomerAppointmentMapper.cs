using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartCustomerAppointmentMapper : Profile
    {
        public SparePartCustomerAppointmentMapper()
        {
            CreateMap<SparePartCustomerAppointmentDTO, SparePartCustomerAppointmentViewModel>();
            CreateMap<SparePartCustomerAppointmentViewModel, SparePartCustomerAppointmentDTO>();
        }
    }
}
