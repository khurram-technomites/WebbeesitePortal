using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageMenuClient
    {
        Task<IEnumerable<GarageMenuDTO>> GetAllAsync();
        Task<IEnumerable<GarageMenuDTO>> GetMenuByGarageId(long Id);
        Task<IEnumerable<GarageMenuDTO>> GetAllByIdAsync(long Id);
        Task<GarageMenuDTO> AddGarageMenuAsync(GarageMenuDTO Entity);
        Task<GarageMenuDTO> UpdateGarageMenuAsync(GarageMenuDTO Entity);
        Task DeleteGarageMenuAsync(long Id);
    }
}
