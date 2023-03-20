using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartContentManagementService
    {
        Task<IEnumerable<SparePartContentManagement>> GetAllAsync();
        Task<IEnumerable<SparePartContentManagement>> GetSparePartContentManagementByIdAsync(long Id);
        Task<IEnumerable<SparePartContentManagement>> GetSparePartContentManagementBySparePartIdAsync(long SparePartId);
        Task<SparePartContentManagement> AddSparePartContentManagementAsync(SparePartContentManagement Model);
        Task<SparePartContentManagement> UpdateSparePartContentManagementAsync(SparePartContentManagement Model);
        Task<SparePartContentManagement> ArchiveSparePartContentManagementAsync(long Id);
    }
}
