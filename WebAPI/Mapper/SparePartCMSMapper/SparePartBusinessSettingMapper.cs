using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.SparePartCMSMapper
{
    public class SparePartBusinessSettingMapper : Profile
    {
        public SparePartBusinessSettingMapper()
        {
            CreateMap<SparePartBusinessSetting, SparePartBusinessSettingDTO>();
            CreateMap<SparePartBusinessSettingDTO, SparePartBusinessSetting>();
        }
    }
}
