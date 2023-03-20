using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageMenuMapper:Profile
    {
        public GarageMenuMapper()
        {
            CreateMap<GarageMenuDTO, GarageMenuViewModel>();
            CreateMap<GarageMenuViewModel, GarageMenuDTO>();
        }
    }
}
