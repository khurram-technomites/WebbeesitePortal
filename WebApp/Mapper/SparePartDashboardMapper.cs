using AutoMapper;
using HelperClasses.DTOs.SparePartDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ViewModels.SparePart;

namespace WebApp.Mapper
{
    public class SparePartDashboardMapper : Profile
    {
        public SparePartDashboardMapper()
        {
            CreateMap<SparePartDashboardStatsDTO, SparePartDashboardViewModel>();
            CreateMap<SparePartDashboardViewModel, SparePartDashboardStatsDTO>();
        }
    }
}
