using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartContentManagementMapper : Profile
    {
        public SparePartContentManagementMapper()
        {
            CreateMap<SparePartContentManagementDTO, SparePartContentManagementViewModel>();
            CreateMap<SparePartContentManagementViewModel, SparePartContentManagementDTO>();
        }
    }
}
