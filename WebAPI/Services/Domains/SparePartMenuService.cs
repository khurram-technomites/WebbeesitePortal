using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartMenuService : ISparePartMenuService
    {
        private readonly ISparePartMenuRepo _repo;
        public SparePartMenuService(ISparePartMenuRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartMenu>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<SparePartMenu>> GetSparePartMenuById(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<SparePartMenu>> GetSparePartMenuBySparepartDealerId(long SparePartDealerId)
        {
            return await _repo.GetMenuBySparePartDealerId(SparePartDealerId);
        }
        public async Task<SparePartMenu> AddSparePartMenuAsync(SparePartMenu Entity)
        {
            return await _repo.InsertAsync(Entity);
        }
        public async Task<SparePartMenu> UpdateSparePartMenuAsync(SparePartMenu Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
        public async Task<SparePartMenu> ArchiveSparePartMenuAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
