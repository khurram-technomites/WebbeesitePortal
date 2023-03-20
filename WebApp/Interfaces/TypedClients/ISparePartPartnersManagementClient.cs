using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartPartnersManagementClient
    {
        Task<IEnumerable<SparePartPartnersManagementDTO>> GetAllAsync();
        Task<IEnumerable<SparePartPartnersManagementDTO>> GetAllByIdAsync(long Id);
        Task<SparePartPartnersManagementDTO> GetMaxPosition(long Id);
        //Task<GaragePartnersManagementDTO> GetMaxPosition(long Id);
        Task<IEnumerable<SparePartPartnersManagementDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartPartnersManagementDTO> AddSparePartPartnersManagementAsync(SparePartPartnersManagementDTO Entity);
        Task<SparePartPartnersManagementDTO> UpdateSparePartPartnersManagementAsync(SparePartPartnersManagementDTO Entity);
        Task DeleteSparePartPartnersManagementAsync(long Id);
        Task<SparePartPartnersManagementDTO> SavePositions(SparePartPartnersManagementDTO Entity);
    }
}
