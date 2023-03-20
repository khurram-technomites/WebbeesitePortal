using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageExpertiseMapper:Profile
    {
        public GarageExpertiseMapper()
        {
            CreateMap<GarageExpertiseViewModel, GarageExpertiseDTO>();
            CreateMap<GarageExpertiseDTO, GarageExpertiseViewModel>();
        }
    }
}
