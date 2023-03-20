using AutoMapper;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels;

namespace WebApp.Mapper 
{
    public class DashboardMapper : Profile
    {
        public DashboardMapper()
        {
            CreateMap<AdminDashboardStatsDTO, DashboardViewModel>();
            CreateMap<DashboardViewModel, AdminDashboardStatsDTO>();
            CreateMap<VendorDashboardStatsDTO, DashboardViewModel>();
            CreateMap<DashboardViewModel, VendorDashboardStatsDTO>();
        }
    }
}
