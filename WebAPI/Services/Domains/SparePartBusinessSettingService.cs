using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartBusinessSettingService : ISparePartBusinessSettingService
    {
        private readonly ISparePartBusinessSettingRepo _repo;
        public SparePartBusinessSettingService(ISparePartBusinessSettingRepo repo)
        {
            _repo = repo;
        }

        public async Task<SparePartBusinessSetting> AddBusinessSettingAsync(SparePartBusinessSetting Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<SparePartBusinessSetting> ArchiveBusinessSettingAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<SparePartBusinessSetting>> GetBusinessSettingBySparePartIdAsync(long SparePartId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartId == SparePartId);
        }

        public async Task<IEnumerable<SparePartBusinessSetting>> GetBusinessSettingByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<SparePartBusinessSetting> UpdateBusinessSettingAsync(SparePartBusinessSetting Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
