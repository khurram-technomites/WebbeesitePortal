using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartTeamManagementClient
    {
        Task<IEnumerable<SparePartTeamManagementDTO>> GetAllAsync();
        Task<IEnumerable<SparePartTeamManagementDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartTeamManagementDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartTeamManagementDTO> AddSparePartTeamManagementAsync(SparePartTeamManagementDTO Entity);
        Task<SparePartTeamManagementDTO> UpdateSparePartTeamManagementAsync(SparePartTeamManagementDTO Entity);
        Task DeleteSparePartTeamManagementAsync(long Id);
    }
}
