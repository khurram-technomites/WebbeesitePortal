using AutoMapper;
using HelperClasses.DTOs.GarageCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageBusinessSettingMapper : Profile
    {
        public GarageBusinessSettingMapper()
        {
            CreateMap<GarageBusinessSettingViewModel, GarageBusinessSettingDTO>();
            CreateMap<GarageBusinessSettingDTO, GarageBusinessSettingViewModel>();

            CreateMap<GarageBranchBusinessSettingViewModel, GarageBranchBusinessSettingDTO>();
            CreateMap<GarageBranchBusinessSettingDTO, GarageBranchBusinessSettingViewModel>();
        }
    }
}
