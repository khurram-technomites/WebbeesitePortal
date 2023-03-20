using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageMenuManagementClient
    {
        Task<IEnumerable<GarageMenuManagementDTO>> GetAllAsync();
        Task<IEnumerable<GarageMenuManagementDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageMenuManagementDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<IEnumerable<GarageMenuManagementDTO>> GetAllByGarageMenuIdAsync(long MenuId);
        Task<GarageMenuManagementDTO> AddGarageMenuManagementAsync(GarageMenuManagementDTO Entity);
        Task<GarageMenuManagementDTO> UpdateGarageMenuManagementAsync(GarageMenuManagementDTO Entity);
        Task DeleteGarageMenuManagementAsync(long Id);
        Task<GarageMenuManagementDTO> SavePositions(GarageMenuManagementDTO Entity);
    }
}
