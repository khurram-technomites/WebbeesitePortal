using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageCustomerFeedbackService: IGarageCustomerFeedbackService
    {
        private readonly IGarageCustomerFeedbackRepo _repo;
        public GarageCustomerFeedbackService(IGarageCustomerFeedbackRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GarageCustomerFeedback>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageCustomerFeedback>> GetGarageCareersByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GarageCustomerFeedback>> GetGarageCustomerFeedbackByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }

        public async Task<GarageCustomerFeedback> AddGarageCustomerFeedbackAsync(GarageCustomerFeedback Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageCustomerFeedback> UpdateGarageCustomerFeedbackAsync(GarageCustomerFeedback Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GarageCustomerFeedback> ArchiveGarageCustomerFeedbackAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
