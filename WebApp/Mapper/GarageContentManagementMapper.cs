using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageContentManagementMapper : Profile
    {
        public GarageContentManagementMapper()
        {
            CreateMap<GarageContentManagementViewModel, GarageContentManagementDTO>();
            CreateMap<GarageContentManagementDTO, GarageContentManagementViewModel>();
        }
    }
}
