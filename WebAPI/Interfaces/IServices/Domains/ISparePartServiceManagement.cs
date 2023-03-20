using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartServiceManagement
    {
        Task<IEnumerable<SparePartServiceManagement>> GetAllAsync();
        Task<IEnumerable<SparePartServiceManagement>> GetSparePartServiceManagementByIdAsync(long Id);
        Task<IEnumerable<SparePartServiceManagement>> GetSparePartServiceManagementBySlugAsync(string Slug);
        Task<IEnumerable<SparePartServiceManagement>> GetSparePartServiceManagementBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartServiceManagement> AddSparePartServiceManagementAsync(SparePartServiceManagement Model);
        Task<SparePartServiceManagement> UpdateSparePartServiceManagementAsync(SparePartServiceManagement Model);
        Task<SparePartServiceManagement> ArchiveSparePartServiceManagementAsync(long Id);
    }
}
