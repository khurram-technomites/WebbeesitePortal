using AutoMapper;
using HelperClasses.DTOs;
using WebApp.ViewModels;
namespace WebAPI.Mapper
{
    public class ClientModulePurchaseTransactionsMapper : Profile
    {
        public ClientModulePurchaseTransactionsMapper()
        {
            CreateMap<ClientModulePurchaseTransactionsViewModel, ClientModulePurchaseTransactionsDTO>();
            CreateMap<ClientModulePurchaseTransactionsDTO, ClientModulePurchaseTransactionsViewModel>();
        }
    }
}
