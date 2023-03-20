using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartBannerSettingMapper : Profile
    {
        public SparePartBannerSettingMapper()
        {
            CreateMap<SparePartBannerSettingDTO, SparePartBannerSettingViewModel>();
            CreateMap<SparePartBannerSettingViewModel, SparePartBannerSettingDTO>();
        }
    }
}
