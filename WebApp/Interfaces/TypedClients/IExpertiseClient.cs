using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IExpertiseClient
    {
        Task<IEnumerable<ExpertiseDTO>> GetAllAsync();
        Task<IEnumerable<ExpertiseDTO>> GetAllByIdAsync(long Id);
        Task<ExpertiseDTO> AddExpertiseAsync(ExpertiseDTO Entity);
        Task<ExpertiseDTO> UpdateExpertiseAsync(ExpertiseDTO Entity);
        Task DeleteExpertiseAsync(long Id);
        Task<ExpertiseDTO> ToggleActiveStatus(long Id);
    }
}
