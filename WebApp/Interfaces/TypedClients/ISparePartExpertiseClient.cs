using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartExpertiseClient
    {
        Task<IEnumerable<SparePartExpertiseDTO>> GetAllAsync();
        Task<IEnumerable<SparePartExpertiseDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartExpertiseDTO>> GetAllBySparePartExpertiseManagementIdAsync(long ManagementId);
        Task<SparePartExpertiseDTO> AddSparePartExpertiseAsync(SparePartExpertiseDTO Entity);
        Task<SparePartExpertiseDTO> UpdateSparePartExpertiseAsync(SparePartExpertiseDTO Entity);
        Task DeleteSparePartExpertiseAsync(long Id);
        Task ArchiveSparePartExpertiseAsync(long Id);
    }
}
