using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartTestimonialService
    {
        Task<IEnumerable<SparePartTestimonial>> GetAllAsync();
        Task<IEnumerable<SparePartTestimonial>> GetSparePartTestimonialByIdAsync(long Id);
        Task<IEnumerable<SparePartTestimonial>> GetSparePartTestimonialBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartTestimonial> AddSparePartTestimonialAsync(SparePartTestimonial Model);
        Task<SparePartTestimonial> UpdateSparePartTestimonialAsync(SparePartTestimonial Model);
        Task<SparePartTestimonial> ArchiveSparePartTestimonialAsync(long Id);
        Task<long> GetCountBySparePartDealerIdAsync(long SparePartDealerId);
    }
}
