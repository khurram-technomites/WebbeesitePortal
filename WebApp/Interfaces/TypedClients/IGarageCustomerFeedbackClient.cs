using HelperClasses.DTOs.GarageCMS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageCustomerFeedbackClient
    {
        Task<IEnumerable<GarageCustomerFeedbackDTO>> GetAllAsync();
        Task<IEnumerable<GarageCustomerFeedbackDTO>> GetAllByIdAsync(long Id);
        Task<IEnumerable<GarageCustomerFeedbackDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<GarageCustomerFeedbackDTO> AddGarageCustomerFeedbackAsync(GarageCustomerFeedbackDTO Entity);
        Task<GarageCustomerFeedbackDTO> UpdateGarageCustomerFeedbackAsync(GarageCustomerFeedbackDTO Entity);
        Task DeleteGarageCustomerFeedbackAsync(long Id);
    }
}
