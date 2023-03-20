using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class IntegrationSettingMapper : Profile
    {
        public IntegrationSettingMapper()
        {
            CreateMap<IntegrationSetting, IntegrationSettingDTO>();
            CreateMap<IntegrationSettingDTO, IntegrationSetting>()
                .ForMember(o => o.Port, y => y.Condition(x => (x.Port != 0)))
                .ForAllOtherMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
