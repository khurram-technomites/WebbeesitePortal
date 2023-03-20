using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartCustomerFeedbackService : ISparePartCustomerFeedbackService
    {
        private readonly ISparePartCustomerFeedbackRepo _repo;
        public SparePartCustomerFeedbackService(ISparePartCustomerFeedbackRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartCustomerFeedback>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SparePartCustomerFeedback>> GetSparePartCustomerByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartCustomerFeedback>> GetSparePartCustomerFeedbackBySparePartDealerIdAsync(long sparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == sparePartDealerId, ChildObjects: "SparePartDealer");
        }

        public async Task<SparePartCustomerFeedback> AddSparePartCustomerFeedbackAsync(SparePartCustomerFeedback Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartCustomerFeedback> UpdateSparePartCustomerFeedbackAsync(SparePartCustomerFeedback Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartCustomerFeedback> ArchiveSparePartCustomerFeedbackAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
