using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class ExpertiseMapper:Profile
    {
        public ExpertiseMapper()
        {
            CreateMap<ExpertiseViewModel, ExpertiseDTO>();
            CreateMap<ExpertiseDTO, ExpertiseViewModel>();
        }
    }
}
