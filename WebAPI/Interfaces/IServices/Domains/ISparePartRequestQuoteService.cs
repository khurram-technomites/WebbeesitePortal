using HelperClasses.DTOs.Garage.Filter;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartRequestQuoteService
    {
        Task<IEnumerable<SparePartRequestQuote>> GetAllAsync();
        Task<IEnumerable<SparePartAvailableRequests>> GetAllByUserAndFilterAsync(string UserId, SparePartQuoteFilter Filter);
        Task<IEnumerable<SparePartRequestQuote>> GetAllQuoteForFilterAsync(long SpareSpareDealerId,SparePartQuoteFilter Filter);
        Task<IEnumerable<SparePartRequestQuote>> GetByIdAsync(long Id);
        Task<IEnumerable<SparePartRequestQuote>> GetBySparePartsDealerIdAsync(long Id);
        Task<IEnumerable<SparePartRequestQuote>> GetBySparePartRequestIdAsync(long Id);
        Task<IEnumerable<SparePartRequestQuote>> GetPendingQuotesBySparePartRequestIdAsync(long Id);
        Task<IEnumerable<SparePartRequestQuote>> GetQuotesBySparePartRequestIdAsync(long Id);
        Task<SparePartRequestQuote> AddSparePartRequestQuoteAsync(SparePartRequestQuote Model);
        Task<SparePartRequestQuote> UpdateSparePartRequestQuoteAsync(SparePartRequestQuote Model);
        Task<SparePartRequestQuote> ArchiveSparePartRequestQuoteAsync(long Id);
        Task<long> GetTotalCount();

    }
}
