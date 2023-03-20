using HelperClasses.DTOs.Garage.Filter;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface ISparePartRequestQuoteRepo : IRepository<SparePartRequestQuote>
    {
        Task<IEnumerable<SparePartAvailableRequests>> GetByUserAndFilter(string UserId, SparePartQuoteFilter Filter);
        Task<IEnumerable<SparePartRequestQuote>> GetByQuoteFilter(long SparePartDealerId, SparePartQuoteFilter Filter);
    }
}
