using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartServiceManagementClient
    {
        Task<IEnumerable<SparePartServiceManagementDTO>> GetAllAsync();
        Task<IEnumerable<SparePartServiceManagementDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartServiceManagementDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartServiceManagementDTO> AddSparePartServiceManagementAsync(SparePartServiceManagementDTO Entity);
        Task<SparePartServiceManagementDTO> UpdateSparePartServiceManagementAsync(SparePartServiceManagementDTO Entity);
        Task<SparePartServiceManagementDTO> ToggleStatus(long Id);
        Task DeleteSparePartServiceManagementAsync(long Id);
    }
}
