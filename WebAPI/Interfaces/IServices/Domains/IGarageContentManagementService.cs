using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageContentManagementService
    {
        Task<IEnumerable<GarageContentManagement>> GetAllAsync();
        Task<IEnumerable<GarageContentManagement>> GetGarageContentManagementByIdAsync(long Id);
        Task<IEnumerable<GarageContentManagement>> GetGarageContentManagementByGarageIdAsync(long GaragedId);
        Task<GarageContentManagement> AddGarageContentManagementAsync(GarageContentManagement Model);
        Task<GarageContentManagement> UpdateGarageContentManagementAsync(GarageContentManagement Model);
        Task<GarageContentManagement> ArchiveGarageContentManagementAsync(long Id);
    }
}
