using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageSubscribersClient
    {
        Task<IEnumerable<GarageSubscribersDTO>> GetAllAsync();
        Task<IEnumerable<GarageSubscribersDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageSubscribersDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<GarageSubscribersDTO> AddGarageSubscribersAsync(GarageSubscribersDTO Entity);
        Task<GarageSubscribersDTO> UpdateGarageSubscribersAsync(GarageSubscribersDTO Entity);
        Task DeleteGarageSubscribersAsync(long Id);
    }
}
