using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageTestimonialsMapper:Profile
    {
        public GarageTestimonialsMapper()
        {
            CreateMap<GarageTestimonialsViewModel, GarageTestimonialsDTO>();
            CreateMap<GarageTestimonialsDTO, GarageTestimonialsViewModel>();
        }
    }
}
