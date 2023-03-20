using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageServiceManagementClient
    {
        Task<IEnumerable<GarageServiceManagementDTO>> GetAllAsync();
        Task<IEnumerable<GarageServiceManagementDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageServiceManagementDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<long> GetCountAllByGarageIdAsync(long GarageId);
        Task<GarageServiceManagementDTO> AddGarageServiceManagementAsync(GarageServiceManagementDTO Entity);
        Task<GarageServiceManagementDTO> UpdateGarageServiceManagementAsync(GarageServiceManagementDTO Entity);
        Task<GarageServiceManagementDTO> ToggleStatus(long Id);
        Task DeleteGarageServiceManagementAsync(long Id);
    }
}
