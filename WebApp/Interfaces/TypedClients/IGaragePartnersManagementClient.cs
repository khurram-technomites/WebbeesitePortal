using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGaragePartnersManagementClient
    {
        Task<IEnumerable<GaragePartnersManagementDTO>> GetAllAsync();
        Task<IEnumerable<GaragePartnersManagementDTO>> GetAllByIdAsync(long Id);
        Task<GaragePartnersManagementDTO> GetMaxPosition(long Id);
        //Task<GaragePartnersManagementDTO> GetMaxPosition(long Id);
        Task<IEnumerable<GaragePartnersManagementDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<long> GetAllCountByGarageIdAsync(long GarageId);
        Task<GaragePartnersManagementDTO> AddGaragePartnersManagementAsync(GaragePartnersManagementDTO Entity);
        Task<GaragePartnersManagementDTO> UpdateGaragePartnersManagementAsync(GaragePartnersManagementDTO Entity);
        Task DeleteGaragePartnersManagementAsync(long Id);
        Task<GaragePartnersManagementDTO> SavePositions(GaragePartnersManagementDTO Entity);

    }
}
