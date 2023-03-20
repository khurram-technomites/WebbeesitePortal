using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
namespace WebAPI.Services.Domains
{
    public class ClientModulePurchaseTransactionsService: IClientModulePurchaseTransactionsService
    {
        private readonly IClientModulePurchaseTransactionsRepo _repo;
        public ClientModulePurchaseTransactionsService(IClientModulePurchaseTransactionsRepo repo)
        {
            _repo = repo;
        }

        public async Task<ClientModulePurchaseTransactions> AddClientModulePurchaseTransactionsAsync(ClientModulePurchaseTransactions Entity)
        {
            return await _repo.InsertAsync(Entity);
        }
        public async Task<IEnumerable<ClientModulePurchaseTransactions>> GetClientModulePurchaseTransactionsByVendorIDAsync(long VendorId)
        {
            return await _repo.GetAllAsync(x=>x.ClientModulePurchases.Garage.VendorId == VendorId, ChildObjects: "ClientModulePurchases,ClientModulePurchases.Garage");
        }
    }
}
