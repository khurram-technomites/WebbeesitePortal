using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageCareersClient
    {
        Task<IEnumerable<GarageCareerDTO>> GetAllAsync();
        Task<IEnumerable<GarageCareerDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageCareerDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<GarageCareerDTO> AddGarageCareerAsync(GarageCareerDTO Entity);
        Task<GarageCareerDTO> UpdateGarageCareerAsync(GarageCareerDTO Entity);
        Task DeleteGarageCareerAsync(long Id);
    }
}
