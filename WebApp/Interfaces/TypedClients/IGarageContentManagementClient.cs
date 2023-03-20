using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageContentManagementClient
    {
        Task<IEnumerable<GarageContentManagementDTO>> GetAllAsync();
        Task<IEnumerable<GarageContentManagementDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageContentManagementDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<GarageContentManagementDTO> AddGarageContentManagementAsync(GarageContentManagementDTO Entity);
        Task<GarageContentManagementDTO> UpdateGarageContentManagementAsync(GarageContentManagementDTO Entity);
        Task DeleteGarageContentManagementAsync(long Id);
    }
}
