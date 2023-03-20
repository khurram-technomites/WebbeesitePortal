using HelperClasses.DTOs;
using HelperClasses.DTOs.CustomerFeedback;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ICustomerFeedbackClient
    {
        Task<IEnumerable<CustomerFeedbackDTO>> GetAllAsync(PagingParameters paging);
        Task<IEnumerable<CustomerFeedbackDTO>> GetAllAsync();
        Task<IEnumerable<CustomerFeedbackDTO>> GetByRestaurantIdAsync(long Id);
        Task<CustomerFeedbackDTO> GetByIdAsync(long Id);
        Task<CustomerFeedbackDTO> AddAsync(CustomerFeedbackDTO Entity);
        Task<CustomerFeedbackDTO> UpdateAsync(CustomerFeedbackDTO Entity);
        Task<CustomerFeedbackDTO> DeleteAsync(long Id);
        Task<CustomerFeedbackDTO> ToggleActiveStatus(long Id);
    }
}