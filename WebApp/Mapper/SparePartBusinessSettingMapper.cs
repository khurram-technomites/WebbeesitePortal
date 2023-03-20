using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartBusinessSettingMapper : Profile
    {
        public SparePartBusinessSettingMapper()
        {
            CreateMap<SparePartBusinessSettingViewModel, SparePartBusinessSettingDTO>();
            CreateMap<SparePartBusinessSettingDTO, SparePartBusinessSettingViewModel>();

            CreateMap<SparePartBranchBusinessSettingViewModel, SparePartBranchBusinessSettingDTO>();
            CreateMap<SparePartBranchBusinessSettingDTO, SparePartBranchBusinessSettingViewModel>();
        }
    }
}
