using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartExpertiseManagementService : ISparePartExpertiseManagementService
    {
        private readonly ISparePartExpertiseManagementRepo _repo;
        public SparePartExpertiseManagementService(ISparePartExpertiseManagementRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartExpertiseManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SparePartExpertiseManagement>> GetSparePartExpertiseManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartExpertiseManagement>> GetSparePartExpertiseManagementBySparePartDealerIdAsync(long SparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == SparePartDealerId, ChildObjects: "SparePartExpertise");
        }

        public async Task<SparePartExpertiseManagement> AddSparePartExpertiseManagementAsync(SparePartExpertiseManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartExpertiseManagement> UpdateSparePartExpertiseManagementAsync(SparePartExpertiseManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartExpertiseManagement> ArchiveSparePartExpertiseManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task DeleteSparePartExpertiseManagementAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }
    }
}
