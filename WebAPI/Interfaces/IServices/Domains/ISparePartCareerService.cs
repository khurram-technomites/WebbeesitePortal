using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartCareerService
    {
        Task<IEnumerable<SparePartCareer>> GetAllAsync();
        Task<IEnumerable<SparePartCareer>> GetSparePartCareerByIdAsync(long Id);
        Task<IEnumerable<SparePartCareer>> GetSparePartCareerBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartCareer> AddSparePartCareerAsync(SparePartCareer Model);
        Task<SparePartCareer> UpdateSparePartCareerAsync(SparePartCareer Model);
        Task<SparePartCareer> ArchiveSparePartCareerAsync(long Id);
    }
}
