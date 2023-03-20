using AutoMapper;
using HelperClasses.DTOs.SparePartCMS;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebApp.Mapper
{
    public class SparePartMenuMapper : Profile
    {
        public SparePartMenuMapper()
        {
            CreateMap<SparePartMenuDTO, SparePartMenuViewModel>();
            CreateMap<SparePartMenuViewModel, SparePartMenuDTO>();
        }
    }
}
