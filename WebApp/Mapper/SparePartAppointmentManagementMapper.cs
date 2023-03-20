using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartAppointmentManagementMapper : Profile
    {
        public SparePartAppointmentManagementMapper()
        {
            CreateMap<SparePartAppointmentManagementDTO, SparePartAppointmentManagementViewModel>();
            CreateMap<SparePartAppointmentManagementViewModel, SparePartAppointmentManagementDTO>();
        }
    }
}
