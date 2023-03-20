using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartExpertiseMapper : Profile
    {
        public SparePartExpertiseMapper()
        {
            CreateMap<SparePartExpertiseDTO, SparePartExpertiseViewModel>();
            CreateMap<SparePartExpertiseViewModel, SparePartExpertiseDTO>();
        }
    }
}
