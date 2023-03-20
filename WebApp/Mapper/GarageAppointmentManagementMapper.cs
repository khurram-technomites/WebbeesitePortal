using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageAppointmentManagementMapper:Profile
    {
        public GarageAppointmentManagementMapper()
        {
            CreateMap<GarageAppointmentManagementDTO, GarageAppointmentManagementViewModel>();
            CreateMap<GarageAppointmentManagementViewModel, GarageAppointmentManagementDTO>();
        }
    }
}
