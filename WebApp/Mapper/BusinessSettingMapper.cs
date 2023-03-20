using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Areas.Admin.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class BusinessSettingMapper : Profile
    {
        public BusinessSettingMapper()
        {
            CreateMap<BusinessSettingDTO, BusinessSettingViewModel>();
            CreateMap<BusinessSettingViewModel, BusinessSettingDTO>();
        }
        
    }
}
