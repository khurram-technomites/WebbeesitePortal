using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartContentManagementService : ISparePartContentManagementService
    {
        private readonly ISparePartContentManagementRepo _repo;
        public SparePartContentManagementService(ISparePartContentManagementRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<SparePartContentManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SparePartContentManagement>> GetSparePartContentManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartContentManagement>> GetSparePartContentManagementBySparePartIdAsync(long SparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == SparePartDealerId, ChildObjects: "SparePartDealer");
        }

        public async Task<SparePartContentManagement> AddSparePartContentManagementAsync(SparePartContentManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartContentManagement> UpdateSparePartContentManagementAsync(SparePartContentManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartContentManagement> ArchiveSparePartContentManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
