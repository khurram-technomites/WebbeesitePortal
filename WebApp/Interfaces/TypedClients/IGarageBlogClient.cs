using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageBlogClient
    {
        Task<IEnumerable<GarageBlogDTO>> GetAllAsync();
        Task<IEnumerable<GarageBlogDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageBlogDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<long> GetCountByGarageIdAsync(long GarageId);
        Task<GarageBlogDTO> AddGarageBlogAsync(GarageBlogDTO Entity);
        Task<GarageBlogDTO> UpdateGarageBlogAsync(GarageBlogDTO Entity);
        Task<GarageBlogDTO> ToggleStatus(long Id);
        Task DeleteGarageBlogAsync(long Id);
    }
}
