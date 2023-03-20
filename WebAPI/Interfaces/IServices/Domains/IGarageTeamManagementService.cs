using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageTeamManagementService
    {
        Task<IEnumerable<GarageTeamManagement>> GetAllAsync();
        Task<IEnumerable<GarageTeamManagement>> GetGarageTeamManagementByIdAsync(long Id);
        Task<IEnumerable<GarageTeamManagement>> GetGarageTeamManagementByGarageIdAsync(long GaragedId);
        Task<long> GetGarageTeamManagementCountByGarageIdAsync(long GaragedId);
        Task<GarageTeamManagement> AddGarageTeamManagementAsync(GarageTeamManagement Model);
        Task<GarageTeamManagement> UpdateGarageTeamManagementAsync(GarageTeamManagement Model);
        Task<GarageTeamManagement> ArchiveGarageTeamManagementAsync(long Id);
    }
}
