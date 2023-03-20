using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartExpertiseManagementClient
    {
        Task<IEnumerable<SparePartExpertiseManagementDTO>> GetAllAsync();
        Task<IEnumerable<SparePartExpertiseManagementDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartExpertiseManagementDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartExpertiseManagementDTO> AddSparePartExpertiseManagementAsync(SparePartExpertiseManagementDTO Entity);
        Task<SparePartExpertiseManagementDTO> UpdateSparePartExpertiseManagementAsync(SparePartExpertiseManagementDTO Entity);
        Task DeleteSparePartExpertiseManagementAsync(long Id);
        Task ArchiveSparePartExpertiseManagementAsync(long Id);
    }
}
