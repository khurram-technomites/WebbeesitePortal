using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageCareersService
    {
        Task<IEnumerable<GarageCareers>> GetAllAsync();
        Task<IEnumerable<GarageCareers>> GetGarageCareersByIdAsync(long Id);
        Task<IEnumerable<GarageCareers>> GetGarageCareersByGarageIdAsync(long GaragedId);
        Task<GarageCareers> AddGarageCareersAsync(GarageCareers Model);
        Task<GarageCareers> UpdateGarageCareersAsync(GarageCareers Model);
        Task<GarageCareers> ArchiveGarageCareersAsync(long Id);
    }
}
