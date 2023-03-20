using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;
namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IModulePurchaseDetailsService
    {
        Task<IEnumerable<ModulePurchaseDetails>> GetDetailsByIdAsync(long Id);
        Task<IEnumerable<ModulePurchaseDetails>> GetDetailsByPurchaseId(long PurchaseId);
        Task<IEnumerable<ModulePurchaseDetails>> GetDetailsByPurchaseIdandName(long PurchaseId,string Name);
        Task<ModulePurchaseDetails> AddAsync(ModulePurchaseDetails Entity);
        Task<IEnumerable<ModulePurchaseDetails>> AddRangeAsync(List<ModulePurchaseDetails>  Entity);
        Task<IEnumerable<ModulePurchaseDetails>> UpdateRangeAsync(List<ModulePurchaseDetails> Entity);

        Task<ModulePurchaseDetails> UpdateAsync(ModulePurchaseDetails Entity);

        Task DeleteAsync(long Id);
    }
}
