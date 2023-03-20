using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageMenuManagementService
    {
        Task<IEnumerable<GarageMenuManagement>> GetAllAsync();
        Task<IEnumerable<GarageMenuManagement>> GetGarageMenuManagementByIdAsync(long Id);
        Task<IEnumerable<GarageMenuManagement>> GetGarageMenuManagementByGaragedIdAsync(long GaragedId);
        Task<IEnumerable<GarageMenuManagement>> GetGarageMenuManagementByMenuIdAsync(long MenuId );
        Task<GarageMenuManagement> AddGarageMenuManagementAsync(GarageMenuManagement Model);
        Task<GarageMenuManagement> UpdateGarageMenuManagementAsync(GarageMenuManagement Model);
        Task<GarageMenuManagement> ArchiveGarageMenuManagementAsync(long Id);
        Task<long> GetCountByGarageIdAsync(long garageId);
    }
}
