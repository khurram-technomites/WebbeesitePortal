using AutoMapper;
using HelperClasses.DTOs;
using WebApp.ViewModels;
namespace WebApp.Mapper
{
    public class ModuleMapper:Profile
    {
        public ModuleMapper()
        {
            CreateMap<ModuleDTO, ModuleViewModel>();
            CreateMap<ModuleViewModel, ModuleDTO>();
        }
    }
}
