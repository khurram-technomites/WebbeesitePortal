using HelperClasses.DTOs.GarageCMS;
using HelperClasses.DTOs.SparePartCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartCustomerFeedbackClient
    {
        Task<IEnumerable<SparePartCustomerFeedbackDTO>> GetAllAsync();
        Task<IEnumerable<SparePartCustomerFeedbackDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<SparePartCustomerFeedbackDTO>> GetAllBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartCustomerFeedbackDTO> AddSparePartCustomerFeedbackAsync(SparePartCustomerFeedbackDTO Entity);
        Task<SparePartCustomerFeedbackDTO> UpdateSparePartCustomerFeedbackAsync(SparePartCustomerFeedbackDTO Entity);
        Task DeleteSparePartCustomerFeedbacksync(long Id);
    }
}
