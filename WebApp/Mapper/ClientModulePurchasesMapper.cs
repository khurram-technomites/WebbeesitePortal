using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;
using WebApp.ViewModels;

namespace WebAPI.Mapper
{
    public class ClientModulePurchasesMapper:Profile
    {
        public ClientModulePurchasesMapper()
        {
            CreateMap<ClientModulePurchasesViewModel, ClientModulePurchasesDTO>();
            CreateMap<ClientModulePurchasesDTO, ClientModulePurchasesViewModel>();
        }
    }
}
