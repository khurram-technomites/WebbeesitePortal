using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebApp.Interfaces.TypedClients;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageCareersMapper:Profile
    {
        public GarageCareersMapper()
        {
            CreateMap<GarageCareersViewModel, GarageCareerDTO>();
            CreateMap<GarageCareerDTO, GarageCareersViewModel>();
        }
    }
}
