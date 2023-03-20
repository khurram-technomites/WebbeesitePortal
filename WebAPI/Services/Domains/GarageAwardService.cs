using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageAwardService : IGarageAwardService
    {
        private readonly IGarageAwardRepo _repo;
        public GarageAwardService(IGarageAwardRepo repo)
        {
            _repo = repo;
        }

        public async Task<GarageAward> AddGarageAwardAsync(GarageAward Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<GarageAward> ArchiveGarageAwardAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<long> GetAllAwardByGarageIdAsync(long garageId)
        {
            return await _repo.GetCount(x => x.GarageId == garageId);
        }
        public async Task<IEnumerable<GarageAward>> GetAllGarageAwardsAsync(long GarageId)
        {
            return await _repo.GetAllAsync(x => x.GarageId == GarageId);
        }
        public async Task<long> GetCountAllGarageAwardsAsync(long GarageId)
        {
            return await _repo.GetCount(x => x.GarageId == GarageId);
        }
        public async Task<IEnumerable<GarageAward>> GetGarageAwardByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<GarageAward> UpdateGarageAwardAsync(GarageAward Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
