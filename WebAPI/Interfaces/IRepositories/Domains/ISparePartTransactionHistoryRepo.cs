using HelperClasses.DTOs.Garage.Filter;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface ISparePartTransactionHistoryRepo : IRepository<SparePartTransactionHistory>
    {
        Task<IEnumerable<SparePartTransactionHistory>> GetByQuoteFilter(long SparePartDealerId, SparePartQuoteWalletFilter Filter);
        Task<decimal> GetWallet(long SparePartDealerId);
    }
}
