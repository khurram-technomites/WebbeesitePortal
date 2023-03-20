using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartServiceManagementMapper : Profile
    {
        public SparePartServiceManagementMapper()
        {
            CreateMap<SparePartServiceManagementDTO, SparePartServiceManagementViewModel>();
            CreateMap<SparePartServiceManagementViewModel, SparePartServiceManagementDTO>();
        }
    }
}
