using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartCareerClient
    {
        Task<IEnumerable<SparePartCareerDTO>> GetAllAsync();
        Task<IEnumerable<SparePartCareerDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartCareerDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartCareerDTO> AddSparePartCareerDTOAsync(SparePartCareerDTO Entity);
        Task<SparePartCareerDTO> UpdateSparePartCareerDTOAsync(SparePartCareerDTO Entity);
        Task DeleteSparePartCareerDTOAsync(long Id);
    }
}
