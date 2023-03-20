using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartExpertiseManagementMapper : Profile
    {
        public SparePartExpertiseManagementMapper()
        {
            CreateMap<SparePartExpertiseManagementDTO, SparePartExpertiseManagementViewModel>();
            CreateMap<SparePartExpertiseManagementViewModel, SparePartExpertiseManagementDTO>();
        }
    }
}
