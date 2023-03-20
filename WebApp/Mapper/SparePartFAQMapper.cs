using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartFAQMapper : Profile
    {
        public SparePartFAQMapper()
        {
            CreateMap<SparePartFAQDTO, SparePartFAQViewModel>();
            CreateMap<SparePartFAQViewModel, SparePartFAQDTO>();
        }
    }
}
