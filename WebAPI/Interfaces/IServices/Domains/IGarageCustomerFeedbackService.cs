using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageCustomerFeedbackService
    {
        Task<IEnumerable<GarageCustomerFeedback>> GetAllAsync();
        Task<IEnumerable<GarageCustomerFeedback>> GetGarageCareersByIdAsync(long Id);
        Task<IEnumerable<GarageCustomerFeedback>> GetGarageCustomerFeedbackByGarageIdAsync(long GaragedId);
        Task<GarageCustomerFeedback> AddGarageCustomerFeedbackAsync(GarageCustomerFeedback Model);
        Task<GarageCustomerFeedback> UpdateGarageCustomerFeedbackAsync(GarageCustomerFeedback Model);
        Task<GarageCustomerFeedback> ArchiveGarageCustomerFeedbackAsync(long Id);
    }
}
