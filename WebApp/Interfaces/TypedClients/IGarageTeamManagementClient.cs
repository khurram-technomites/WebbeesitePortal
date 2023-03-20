using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageTeamManagementClient
    {
        Task<IEnumerable<GarageTeamManagementDTO>> GetAllAsync();
        Task<IEnumerable<GarageTeamManagementDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageTeamManagementDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<long> GetCountAllByGarageIdAsync(long GarageId);
        Task<GarageTeamManagementDTO> AddGarageTeamManagementAsync(GarageTeamManagementDTO Entity);
        Task<GarageTeamManagementDTO> UpdateGarageTeamManagementAsync(GarageTeamManagementDTO Entity);
        Task DeleteGarageTeamManagementAsync(long Id);
    }
}
