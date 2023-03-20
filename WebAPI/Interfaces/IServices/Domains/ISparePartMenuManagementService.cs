using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartMenuManagementService
    {
        Task<IEnumerable<SparePartMenuManagement>> GetAllAsync();
        Task<IEnumerable<SparePartMenuManagement>> GetSparePartMenuManagementByIdAsync(long Id);
        Task<IEnumerable<SparePartMenuManagement>> GetSparePartMenuManagementBySparePartIdAsync(long SparePartId);
        Task<IEnumerable<SparePartMenuManagement>> GetSparePartMenuManagementByMenuIdAsync(long MenuId);
        Task<SparePartMenuManagement> AddSparePartMenuManagementAsync(SparePartMenuManagement Model);
        Task<SparePartMenuManagement> UpdateSparePartMenuManagementAsync(SparePartMenuManagement Model);
        Task<SparePartMenuManagement> ArchiveSparePartMenuManagementAsync(long Id);
        Task<long> GetCountBySparePartIdAsync(long SparePartId);
    }
}
