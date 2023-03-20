using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageExpertiseClient
    {
        Task<IEnumerable<GarageExpertiseDTO>> GetAllAsync();
        Task<IEnumerable<GarageExpertiseDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageExpertiseDTO>> GetAllByGarageExpertiseManagementIdAsync(long ManagementId);
        Task<GarageExpertiseDTO> AddGarageExpertiseAsync(GarageExpertiseDTO Entity);
        Task<GarageExpertiseDTO> UpdateGarageExpertiseAsync(GarageExpertiseDTO Entity);
        Task DeleteGarageExpertiseAsync(long Id);
        Task ArchiveGarageExpertiseAsync(long Id);
    }
}
