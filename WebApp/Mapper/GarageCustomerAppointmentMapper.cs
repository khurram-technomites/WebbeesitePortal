using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageCustomerAppointmentMapper:Profile
    {
        public GarageCustomerAppointmentMapper()
        {
            CreateMap<GarageCustomerAppointmentViewModel, GarageCustomerAppointmentDTO>();
            CreateMap<GarageCustomerAppointmentDTO, GarageCustomerAppointmentViewModel>();
        }
    }
}
