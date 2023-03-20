using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageServiceManagementService
    {
        Task<IEnumerable<GarageServiceManagement>> GetAllAsync();
        Task<IEnumerable<GarageServiceManagement>> GetGarageServiceManagementByIdAsync(long Id);
        Task<IEnumerable<GarageServiceManagement>> GetGarageServiceManagementBySlugAsync(string Slug);
        Task<IEnumerable<GarageServiceManagement>> GetGarageServiceManagementByGaragedIdAsync(long GaragedId);
        Task<long> GetGarageServiceManagementCountByGaragedIdAsync(long GaragedId);
        Task<GarageServiceManagement> AddGarageServiceManagementAsync(GarageServiceManagement Model);
        Task<GarageServiceManagement> UpdateGarageServiceManagementAsync(GarageServiceManagement Model);
        Task<GarageServiceManagement> ArchiveGarageServiceManagementAsync(long Id);
    }
}
