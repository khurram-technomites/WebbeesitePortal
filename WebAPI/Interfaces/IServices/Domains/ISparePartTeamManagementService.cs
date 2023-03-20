using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartTeamManagementService
    {
        Task<IEnumerable<SparePartTeamManagement>> GetAllAsync();
        Task<IEnumerable<SparePartTeamManagement>> GetSparePartTeamManagementByIdAsync(long Id);
        Task<IEnumerable<SparePartTeamManagement>> GetSparePartTeamManagementBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartTeamManagement> AddSparePartTeamManagementtAsync(SparePartTeamManagement Model);
        Task<SparePartTeamManagement> UpdateSparePartTeamManagementAsync(SparePartTeamManagement Model);
        Task<SparePartTeamManagement> ArchiveSparePartTeamManagementAsync(long Id);
    }
}
