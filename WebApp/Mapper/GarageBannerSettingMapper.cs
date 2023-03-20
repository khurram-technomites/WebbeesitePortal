using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageBannerSettingMapper : Profile
    {
        public GarageBannerSettingMapper()
        {
            CreateMap<GarageBannerSettingViewModel, GarageBannerSettingDTO>();
            CreateMap<GarageBannerSettingDTO, GarageBannerSettingViewModel>();
        }
    }
}
