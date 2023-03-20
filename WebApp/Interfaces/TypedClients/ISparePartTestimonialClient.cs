using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartTestimonialClient
    {
        Task<IEnumerable<SparePartTestimonialDTO>> GetAllAsync();
        Task<IEnumerable<SparePartTestimonialDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartTestimonialDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartTestimonialDTO> AddSparePartTestimonialAsync(SparePartTestimonialDTO Entity);
        Task<SparePartTestimonialDTO> UpdateGSparePartTestimonialAsync(SparePartTestimonialDTO Entity);
        Task DeleteSparePartTestimonialAsync(long Id);
        Task<SparePartTestimonialDTO> ToggleActiveStatus(long Id);
    }
}
