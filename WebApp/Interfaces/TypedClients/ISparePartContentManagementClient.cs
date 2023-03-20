using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartContentManagementClient
    {
        Task<IEnumerable<SparePartContentManagementDTO>> GetAllAsync();
        Task<SparePartContentManagementDTO> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartContentManagementDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartContentManagementDTO> AddSparePartContentManagementAsync(SparePartContentManagementDTO Entity);
        Task<SparePartContentManagementDTO> UpdateSparePartContentManagementAsync(SparePartContentManagementDTO Entity);
        Task DeleteSparePartContentManagementAsync(long Id);
    }
}
