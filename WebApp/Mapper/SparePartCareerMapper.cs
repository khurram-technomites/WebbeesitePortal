using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartCareerMapper : Profile
    {
        public SparePartCareerMapper()
        {
            CreateMap<SparePartCareerDTO, SparePartCareerViewModel>();
            CreateMap<SparePartCareerViewModel, SparePartCareerDTO>();
        }
    }
}
