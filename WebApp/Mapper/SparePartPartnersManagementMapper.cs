using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartPartnersManagementMapper : Profile
    {
        public SparePartPartnersManagementMapper()
        {
            CreateMap<SparePartPartnersManagementDTO, SparePartPartnersManagementViewModel>();
            CreateMap<SparePartPartnersManagementViewModel, SparePartPartnersManagementDTO>();
        }
    }
}
