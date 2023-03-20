using HelperClasses.DTOs.Garage;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartFAQClient
    {
        Task<SparePartFAQDTO> AddFAQAsync(SparePartFAQDTO Model);
        Task<SparePartFAQDTO> UpdateFAQAsync(SparePartFAQDTO Model);
        Task<SparePartFAQDTO> SavePosition(SparePartFAQDTO Model);
        Task<SparePartFAQDTO> ArchiveFAQAsync(long Id);
        Task<IEnumerable<SparePartFAQDTO>> GetFAQBySparePartIdAsync(long SparePartId);
        Task<SparePartFAQDTO> GetFAQByIdAsync(long Id);
    }
}
