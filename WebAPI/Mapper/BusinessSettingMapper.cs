using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Mapper
{
    public class BusinessSettingMapper : Profile
    {
        public BusinessSettingMapper()
        {
            CreateMap<BusinessSettings, BusinessSettingDTO>();
            CreateMap<BusinessSettingDTO, BusinessSettings>();
        }
    }
}
