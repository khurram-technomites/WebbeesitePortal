using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface  IClientModulePurchasesClient
    {
        Task<IEnumerable<ClientModulePurchasesDTO>> GetPurchaseByGarageId(long GarageId);
        Task<ClientModulePurchasesDTO> GetPurchaseByID(long Id);
        Task<ClientModulePurchasesDTO> Create(ClientModulePurchasesDTO model);
        Task<ClientModulePurchasesDTO> Edit(ClientModulePurchasesDTO model);

        Task<object> Paid(long InvoiceId, string PaymentId);
        Task<object> GenerateInvoice(ClientModulePurchasesDTO model);
    }
}
