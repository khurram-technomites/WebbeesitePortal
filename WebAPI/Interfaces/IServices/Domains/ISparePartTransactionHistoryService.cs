using HelperClasses.DTOs.Garage.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartTransactionHistoryService
    {
        Task<IEnumerable<SparePartTransactionHistory>> MyWallet(long Id);
        Task<IEnumerable<SparePartTransactionHistory>> GetAllTransactionHistory();
        Task<decimal> getWallet(long SparePartDealerId);
        Task<IEnumerable<SparePartTransactionHistory>> MyWalletFilter(long SparePartDealerId ,DateTime StartDate, DateTime EndDate);
        Task<IEnumerable<SparePartTransactionHistory>> GetAllQuoteForFilterAsync(long SpareSpareDealerId, SparePartQuoteWalletFilter Filter);
    }
}
