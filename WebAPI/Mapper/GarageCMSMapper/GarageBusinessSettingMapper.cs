using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper.GarageCMSMapper
{
    public class GarageBusinessSettingMapper : Profile
    {
        public GarageBusinessSettingMapper()
        {
            CreateMap<GarageBusinessSetting, GarageBusinessSettingDTO>();
            CreateMap<GarageBusinessSettingDTO, GarageBusinessSetting>()
                .ForAllOtherMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
