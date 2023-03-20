using HelperClasses.Classes;
using HelperClasses.DTOs.Garage.Filter;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartTransactionHistoryService : ISparePartTransactionHistoryService
    {
        private readonly ISparePartTransactionHistoryRepo _repo;

        public SparePartTransactionHistoryService(ISparePartTransactionHistoryRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartTransactionHistory>> MyWallet(long SparePartsDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartsDealerId == SparePartsDealerId, ChildObjects: "SparePartsDealer , SparePartRequestQuote" , OrderExp :x=> x.CreationDate , IsOrderByDescending : true);
        }
        public async Task<IEnumerable<SparePartTransactionHistory>> GetAllTransactionHistory()
        {
            return await _repo.GetAllAsync(OrderExp: x => x.CreationDate, ChildObjects: "SparePartsDealer , SparePartRequestQuote" );
        }
        public async Task<IEnumerable<SparePartTransactionHistory>> MyWalletFilter(long SparePartDealerId,DateTime StartDate , DateTime EndDate)
        {
            if (SparePartDealerId != 0 && StartDate.ToString() != "1/1/0001 12:00:00 AM" && EndDate.ToString() != "1/1/0001 12:00:00 AM")
            {
                return await _repo.GetAllAsync(x => x.SparePartsDealerId == SparePartDealerId && x.CreationDate.Date >= StartDate.Date && x.CreationDate.Date <= EndDate.Date);
            }
            else if (SparePartDealerId != 0 && StartDate.ToString() != "1/1/0001 12:00:00 AM" )
            {
                return await _repo.GetAllAsync(x => x.SparePartsDealerId == SparePartDealerId && x.CreationDate == StartDate.Date);
            }
            else if (SparePartDealerId != 0 && EndDate.ToString() != "1/1/0001 12:00:00 AM")
            {
                return await _repo.GetAllAsync(x => x.SparePartsDealerId == SparePartDealerId && x.CreationDate == EndDate.Date);
            }
            else
            {
                return await _repo.GetAllAsync();
            }
        }
        public async Task<IEnumerable<SparePartTransactionHistory>> GetAllQuoteForFilterAsync(long SpareSpareDealerId, SparePartQuoteWalletFilter Filter)
        {
            return await _repo.GetByQuoteFilter(SpareSpareDealerId, Filter);
        }
        public async Task<decimal> getWallet(long SpareSpareDealerId)
        {
            return await _repo.GetWallet(SpareSpareDealerId);
        }
    }
}
