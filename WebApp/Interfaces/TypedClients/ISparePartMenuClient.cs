using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartMenuClient
    {
        Task<IEnumerable<SparePartMenuDTO>> GetAllAsync();
        Task<IEnumerable<SparePartMenuDTO>> GetMenuBySparePartId(long Id);
        Task<IEnumerable<SparePartMenuDTO>> GetAllByIdAsync(long Id);
        Task<SparePartMenuDTO> AddSparePartMenuAsync(SparePartMenuDTO Entity);
        Task<SparePartMenuDTO> UpdateSparePartMenuAsync(SparePartMenuDTO Entity);
        Task DeleteSparePartMenuAsync(long Id);
    }
}
