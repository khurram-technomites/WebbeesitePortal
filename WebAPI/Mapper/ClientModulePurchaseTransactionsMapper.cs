using AutoMapper;
using HelperClasses.DTOs;
using WebAPI.Models;
namespace WebAPI.Mapper
{
    public class ClientModulePurchaseTransactionsMapper : Profile
    {
        public ClientModulePurchaseTransactionsMapper()
        {
            CreateMap<ClientModulePurchaseTransactions, ClientModulePurchaseTransactionsDTO>();
            CreateMap<ClientModulePurchaseTransactionsDTO, ClientModulePurchaseTransactions>();
        }
    }
}
