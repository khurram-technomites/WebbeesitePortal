using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using WebAPI.Repositories.Domains;

namespace WebAPI.Services.Domains
{
    public class GarageMenuService: IGarageMenuService
    {
        private readonly IGarageMenuRepo _repo;

        public GarageMenuService(IGarageMenuRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<GarageMenu>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<GarageMenu>> GetGarageMenuByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<GarageMenu> AddGarageMenuAsync(GarageMenu Entity)
        {
            return await _repo.InsertAsync(Entity);
        }
        public async Task<GarageMenu> UpdateGarageMenuAsync(GarageMenu Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }

        public async Task<IEnumerable<GarageMenu>> GetMenuByGarageId(long GarageId)
        {
            return await _repo.GetMenuByGarageId(GarageId);
        }
        public async Task<GarageMenu> ArchiveGarageMenuAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

    }
}
