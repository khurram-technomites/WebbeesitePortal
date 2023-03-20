using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IExpertiseService
    {
        Task<IEnumerable<Expertise>> GetAllAsync();
        Task<IEnumerable<Expertise>> GetExpertiseByIdAsync(long Id);
        Task<Expertise> AddExpertiseAsync(Expertise Model);
        Task<Expertise> UpdateExpertiseAsync(Expertise Model);
        Task<Expertise> ArchiveExpertiseAsync(long Id);
    }
}
