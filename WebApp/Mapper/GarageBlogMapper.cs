using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageBlogMapper:Profile
    {
        public GarageBlogMapper()
        {
            CreateMap<GarageBlogViewModel, GarageBlogDTO>();
            CreateMap<GarageBlogDTO, GarageBlogViewModel>();
        }
    }
}
