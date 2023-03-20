using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageSubscribersMapper:Profile
    {
        public GarageSubscribersMapper()
        {
            CreateMap<GarageSubscribersDTO, GarageSubscribersViewModel>();
            CreateMap<GarageSubscribersViewModel, GarageSubscribersDTO>();
        }
    }
}
