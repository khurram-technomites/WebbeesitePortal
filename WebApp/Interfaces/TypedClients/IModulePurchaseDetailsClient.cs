using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace WebApp.Interfaces.TypedClients
{
    public interface  IModulePurchaseDetailsClient
    {
        Task<IEnumerable<ModulePurchaseDetailsDTO>> GetDetailsByPurchaseId(long PurchaseId);
        Task<int> GetDetailsByPurchaseIdAndName(long PurchaseId,string Name);
        Task<ModulePurchaseDetailsDTO> GetDetailsByID(long Id);
        Task<ModulePurchaseDetailsDTO> Create(ModulePurchaseDetailsDTO model);

        Task<List<ModulePurchaseDetailsDTO>> CreateRange(List<ModulePurchaseDetailsDTO> model);
        Task<List<ModulePurchaseDetailsDTO>> UpdateRange(List<ModulePurchaseDetailsDTO> model);
        Task<ModulePurchaseDetailsDTO> Edit(ModulePurchaseDetailsDTO model);

        Task Delete(long Id);
    }
}
