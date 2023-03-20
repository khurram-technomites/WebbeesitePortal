using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageBannerSettingMapper : Profile
    {
        public GarageBannerSettingMapper()
        {
            CreateMap<GarageBannerSettingDTO, GarageBannerSetting>();
            CreateMap<GarageBannerSetting, GarageBannerSettingDTO>();
        }
    }
}
