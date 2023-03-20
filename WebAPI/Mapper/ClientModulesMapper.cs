using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;
namespace WebAPI.Mapper
{
    public class ClientModulesMapper:Profile
    {
        public ClientModulesMapper()
        {
            CreateMap<ClientModules, ClientModulesDTO>();
            CreateMap<ClientModulesDTO, ClientModules>();
        }
    }
}
