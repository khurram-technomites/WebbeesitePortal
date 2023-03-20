using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartSubscriberClient
    {
        Task<IEnumerable<SparePartSubscriberDTO>> GetAllAsync();
        Task<IEnumerable<SparePartSubscriberDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartSubscriberDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartSubscriberDTO> AddSparePartSubscriberAsync(SparePartSubscriberDTO Entity);
        Task<SparePartSubscriberDTO> UpdateSparePartSubscriberAsync(SparePartSubscriberDTO Entity);
        Task DeleteSparePartSubscriberAsync(long Id);
    }
}
