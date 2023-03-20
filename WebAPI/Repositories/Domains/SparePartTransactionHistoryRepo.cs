using HelperClasses.Classes;
using HelperClasses.DTOs.Garage.Filter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartTransactionHistoryRepo : Repository<SparePartTransactionHistory>, ISparePartTransactionHistoryRepo
    {
        private readonly new FougitoContext _context;
        public SparePartTransactionHistoryRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {
            _context = context;
        }
        public async Task<IEnumerable<SparePartTransactionHistory>> GetByQuoteFilter(long SparePartDealerId, SparePartQuoteWalletFilter Filter)
        {
            var result = _context.SparePartTransactionHistory.Where(x => x.SparePartsDealerId == SparePartDealerId).Include(x => x.SparePartRequestQuote).Include(x => x.SparePartsDealer).AsQueryable();

            if (Filter.StartDate.HasValue && Filter.EndDate.HasValue)
                result = result.Where(x => x.CreationDate.Date >= Filter.StartDate.Value.Date && x.CreationDate.Date <= Filter.EndDate.Value.Date);
            return result;
        }
        public async Task<decimal> GetWallet(long SparePartDealerId)
        {
            var result = _context.SparePartTransactionHistory.Where(x => x.SparePartsDealerId == SparePartDealerId && x.TransactionStatus == Enum.GetName(typeof(GarageCustomerStatus), GarageCustomerStatus.UnPaid)).DefaultIfEmpty().Sum(x =>Convert.ToDecimal(x.Amount));
            return result;
        }
    }
}
