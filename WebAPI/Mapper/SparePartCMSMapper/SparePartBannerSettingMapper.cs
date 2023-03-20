using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartBannerSettingMapper : Profile
    {
        public SparePartBannerSettingMapper()
        {
            CreateMap<SparePartBannerSetting, SparePartBannerSettingDTO>();
            CreateMap<SparePartBannerSettingDTO, SparePartBannerSetting>();
        }
    }
}
