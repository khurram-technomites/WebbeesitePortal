using NLog;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageExpertiseManagementService
    {
        Task<IEnumerable<GarageExpertiseManagement>> GetAllAsync();
        Task<IEnumerable<GarageExpertiseManagement>> GetGarageExpertiseManagementByIdAsync(long Id);
        Task<IEnumerable<GarageExpertiseManagement>> GetGarageExpertiseManagementByGarageIdAsync(long GaragedId);
        Task<long> GetGarageExpertiseCountByGarageIdAsnyc(long GarageId);
        Task<GarageExpertiseManagement> AddGarageExpertiseManagementAsync(GarageExpertiseManagement Model);
        Task<GarageExpertiseManagement> UpdateGarageExpertiseManagementAsync(GarageExpertiseManagement Model);
        Task<GarageExpertiseManagement> ArchiveGarageExpertiseManagementAsync(long Id);
        Task DeleteGarageExpertiseManagementAsync(long Id);
    }
}
