using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartExpertiseManagementService
    {
        Task<IEnumerable<SparePartExpertiseManagement>> GetAllAsync();
        Task<IEnumerable<SparePartExpertiseManagement>> GetSparePartExpertiseManagementByIdAsync(long Id);
        Task<IEnumerable<SparePartExpertiseManagement>> GetSparePartExpertiseManagementBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartExpertiseManagement> AddSparePartExpertiseManagementAsync(SparePartExpertiseManagement Model);
        Task<SparePartExpertiseManagement> UpdateSparePartExpertiseManagementAsync(SparePartExpertiseManagement Model);
        Task<SparePartExpertiseManagement> ArchiveSparePartExpertiseManagementAsync(long Id);
        Task DeleteSparePartExpertiseManagementAsync(long Id);
    }
}
