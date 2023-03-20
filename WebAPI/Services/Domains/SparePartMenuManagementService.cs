using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartMenuManagementService : ISparePartMenuManagementService
    {
        private readonly ISparePartMenuManagementRepo _repo;
        public SparePartMenuManagementService(ISparePartMenuManagementRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartMenuManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync(ChildObjects: "SparePartMenu");
        }
        public async Task<IEnumerable<SparePartMenuManagement>> GetSparePartMenuManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartMenuManagement>> GetSparePartMenuManagementBySparePartIdAsync(long SparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == SparePartDealerId, ChildObjects: "SparePartsDealer,SparePartMenu");
        }

        public async Task<long> GetCountBySparePartIdAsync(long SparePartId)
        {
            return await _repo.GetCount(x => x.SparePartDealerId == SparePartId);
        }

        public async Task<IEnumerable<SparePartMenuManagement>> GetSparePartMenuManagementByMenuIdAsync(long SparePartMenuId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartMenuId == SparePartMenuId, ChildObjects: "SparePartMenu");
        }

        public async Task<SparePartMenuManagement> AddSparePartMenuManagementAsync(SparePartMenuManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartMenuManagement> UpdateSparePartMenuManagementAsync(SparePartMenuManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartMenuManagement> ArchiveSparePartMenuManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
