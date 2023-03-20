using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageExpertiseManagementClient
    {
        Task<IEnumerable<GarageExpertiseManagementDTO>> GetAllAsync();
        Task<IEnumerable<GarageExpertiseManagementDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageExpertiseManagementDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<long> GetCountAllByGarageIdAsync(long GarageId);
        Task<GarageExpertiseManagementDTO> AddGarageExpertiseManagementAsync(GarageExpertiseManagementDTO Entity);
        Task<GarageExpertiseManagementDTO> UpdateGarageExpertiseManagementAsync(GarageExpertiseManagementDTO Entity);
        Task DeleteGarageExpertiseManagementAsync(long Id);
        Task ArchiveGarageExpertiseManagementAsync(long Id);
    }
}
