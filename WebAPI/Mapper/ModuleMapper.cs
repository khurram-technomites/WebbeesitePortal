using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;
namespace WebAPI.Mapper
{
    public class ModuleMapper:Profile
    {
        public ModuleMapper()
        {
            CreateMap<Module, ModuleDTO>();
            CreateMap<ModuleDTO, Module>()
                .ForAllMembers(o => o.Condition((source, destination, member) => member != null));
        }
    }
}
