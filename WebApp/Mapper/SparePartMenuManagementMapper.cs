using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartMenuManagementMapper : Profile
    {
        public SparePartMenuManagementMapper()
        {
            CreateMap<SparePartMenuManagementDTO, SparePartMenuManagementViewModel>();
            CreateMap<SparePartMenuManagementViewModel, SparePartMenuManagementDTO>();
        }
    }
}
