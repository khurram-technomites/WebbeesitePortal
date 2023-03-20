using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IClientModulePurchaseTransactionsService
    {
        Task<ClientModulePurchaseTransactions> AddClientModulePurchaseTransactionsAsync(ClientModulePurchaseTransactions Entity);
        Task<IEnumerable<ClientModulePurchaseTransactions>> GetClientModulePurchaseTransactionsByVendorIDAsync(long VendorId);
    }
}
