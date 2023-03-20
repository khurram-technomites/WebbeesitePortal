using AutoMapper;
using HelperClasses.DTOs.GarageDashboard;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class GarageDashboardMapper:Profile
    {
        public GarageDashboardMapper()
        {
            CreateMap<GarageDashboardStatsDTO, GarageDashboardViewModel>();
            CreateMap<GarageDashboardViewModel, GarageDashboardStatsDTO>();
        }
    }
}
