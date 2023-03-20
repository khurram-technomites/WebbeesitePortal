using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartFAQService
    {
        Task<SparePartFAQ> AddFAQAsync(SparePartFAQ Model);
        Task<SparePartFAQ> UpdateFAQAsync(SparePartFAQ Model);
        Task<long> MaxCount(long SparePartId);
        Task<SparePartFAQ> ArchiveFAQAsync(long Id);
        Task<IEnumerable<SparePartFAQ>> GetFAQBySparePartAsync(long SparePartId);
        Task<IEnumerable<SparePartFAQ>> GetFAQByIdAsync(long Id);
    }
}
