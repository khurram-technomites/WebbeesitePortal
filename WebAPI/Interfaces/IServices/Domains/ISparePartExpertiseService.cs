using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartExpertiseService
    {
        Task<IEnumerable<SparePartExpertise>> GetAllAsync();
        Task<IEnumerable<SparePartExpertise>> GetSparePartExpertiseByIdAsync(long Id);
        Task<IEnumerable<SparePartExpertise>> GetSparePartExpertiseBySparePartExpertiseManagementIdAsync(long SparePartExpertiseManagementId);
        Task<SparePartExpertise> AddSparePartExpertiseAsync(SparePartExpertise Model);
        Task<SparePartExpertise> UpdateSparePartExpertiseAsync(SparePartExpertise Model);
        Task<SparePartExpertise> ArchiveSparePartExpertiseAsync(long Id);
        Task DeleteSparePartExpertiseAsync(long Id);
    }
}
