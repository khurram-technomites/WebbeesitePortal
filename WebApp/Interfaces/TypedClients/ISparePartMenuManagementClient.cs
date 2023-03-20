using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartMenuManagementClient
    {
        Task<IEnumerable<SparePartMenuManagementDTO>> GetAllAsync();
        Task<IEnumerable<SparePartMenuManagementDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartMenuManagementDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<IEnumerable<SparePartMenuManagementDTO>> GetAllBySparePartMenuIdAsync(long MenuId);
        Task<SparePartMenuManagementDTO> AddSparePartMenuManagementAsync(SparePartMenuManagementDTO Entity);
        Task<SparePartMenuManagementDTO> UpdateSparePartMenuManagementAsync(SparePartMenuManagementDTO Entity);
        Task DeleteSparePartMenuManagementAsync(long Id);
        Task<SparePartMenuManagementDTO> SavePositions(SparePartMenuManagementDTO Entity);
    }
}
