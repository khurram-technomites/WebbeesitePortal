using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartBlogClient
    {
        Task<IEnumerable<SparePartBlogDTO>> GetAllAsync();
        Task<IEnumerable<SparePartBlogDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartBlogDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartBlogDTO> AddSparePartBlogAsync(SparePartBlogDTO Entity);
        Task<SparePartBlogDTO> UpdateSparePartBlogAsync(SparePartBlogDTO Entity);
        Task<SparePartBlogDTO> ToggleStatus(long Id);
        Task DeleteSparePartBlogAsync(long Id);
    }
}
