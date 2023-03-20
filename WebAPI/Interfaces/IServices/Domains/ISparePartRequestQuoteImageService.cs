using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartRequestQuoteImageService
    {
        Task<IEnumerable<SparePartRequestQuoteImage>> GetRequestByImageAsync(string Path);
        Task<IEnumerable<SparePartRequestQuoteImage>> GetBySparePartRequestQuoteIdAsync(long Id);
        Task DeleteAsync(long Id);
    }
}
