using AutoMapper;
using HelperClasses.DTOs;
using WebApp.ViewModels;
namespace WebApp.Mapper
{
    public class ClientModulesMapper:Profile
    {
        public ClientModulesMapper()
        {
            CreateMap<ClientModulesViewModel, ClientModulesDTO>();
            CreateMap<ClientModulesDTO, ClientModulesViewModel>();
        }
    }
}
