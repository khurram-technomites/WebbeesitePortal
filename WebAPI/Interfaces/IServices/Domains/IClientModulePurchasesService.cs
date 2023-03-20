using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;
namespace WebAPI.Interfaces.IServices.Domains
{
    public interface  IClientModulePurchasesService
    {
        Task<IEnumerable<ClientModulePurchases>> GetPurchaseByIdAsync(long Id);
        Task<IEnumerable<ClientModulePurchases>> GetPurchaseByGarageId(long GarageID);
        Task<ClientModulePurchases> AddAsync(ClientModulePurchases Entity);
        Task<ClientModulePurchases> UpdateAsync(ClientModulePurchases Entity);
        Task<IEnumerable<ClientModulePurchases>> GetEarning(long VendorId);
    }
}
