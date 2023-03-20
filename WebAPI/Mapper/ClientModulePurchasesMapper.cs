using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;
namespace WebAPI.Mapper
{
    public class ClientModulePurchasesMapper:Profile
    {
        public ClientModulePurchasesMapper()
        {
            CreateMap<ClientModulePurchases, ClientModulePurchasesDTO>();
            CreateMap<ClientModulePurchasesDTO, ClientModulePurchases>();
        }
    }
}
