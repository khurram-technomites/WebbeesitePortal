using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageBusinessSettingService : IGarageBusinessSettingService
    {
        private readonly IGarageBusinessSettingRepo _repo;
        public GarageBusinessSettingService(IGarageBusinessSettingRepo repo)
        {
            _repo = repo;
        }

        public async Task<GarageBusinessSetting> AddBusinessSettingAsync(GarageBusinessSetting Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<GarageBusinessSetting> ArchiveBusinessSettingAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<GarageBusinessSetting>> GetBusinessSettingByGarageIdAsync(long garageId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == garageId);
        }

        public async Task<IEnumerable<GarageBusinessSetting>> GetBusinessSettingByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        } 
        public async Task<IEnumerable<GarageBusinessSetting>> GetBusinessSettingByGarageId(long garageId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == garageId);
        }

        public async Task<GarageBusinessSetting> UpdateBusinessSettingAsync(GarageBusinessSetting Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
