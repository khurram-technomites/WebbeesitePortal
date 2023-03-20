using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace WebApp.Interfaces.TypedClients
{
    public interface IClientModulePurchaseTransactionsClient
    {
        Task<IEnumerable<ClientModulePurchaseTransactionsDTO>> GetAllTransactionsAsync(long VendorId);
       
    }
}
