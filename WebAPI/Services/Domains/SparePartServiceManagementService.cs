using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.Repositories.Domains;

namespace WebAPI.Services.Domains
{
    public class SparePartServiceManagementService : ISparePartServiceManagement
    {
        private readonly ISparePartServiceManagementRepo _repo;
        public SparePartServiceManagementService(ISparePartServiceManagementRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<SparePartServiceManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SparePartServiceManagement>> GetSparePartServiceManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartServiceManagement>> GetSparePartServiceManagementBySparePartDealerIdAsync(long sparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == sparePartDealerId, ChildObjects: "SparePartDealer");
        }

        public async Task<SparePartServiceManagement> AddSparePartServiceManagementAsync(SparePartServiceManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartServiceManagement> UpdateSparePartServiceManagementAsync(SparePartServiceManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartServiceManagement> ArchiveSparePartServiceManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<SparePartServiceManagement>> GetSparePartServiceManagementBySlugAsync(string Slug)
        {
            return await _repo.GetByIdAsync(x => x.Slug == Slug);
        }
    }
}
