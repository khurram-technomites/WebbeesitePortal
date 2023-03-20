using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartCustomerFeedbackService
    {
        Task<IEnumerable<SparePartCustomerFeedback>> GetAllAsync();
        Task<IEnumerable<SparePartCustomerFeedback>> GetSparePartCustomerByIdAsync(long Id);
        Task<IEnumerable<SparePartCustomerFeedback>> GetSparePartCustomerFeedbackBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartCustomerFeedback> AddSparePartCustomerFeedbackAsync(SparePartCustomerFeedback Model);
        Task<SparePartCustomerFeedback> UpdateSparePartCustomerFeedbackAsync(SparePartCustomerFeedback Model);
        Task<SparePartCustomerFeedback> ArchiveSparePartCustomerFeedbackAsync(long Id);
    }
}
