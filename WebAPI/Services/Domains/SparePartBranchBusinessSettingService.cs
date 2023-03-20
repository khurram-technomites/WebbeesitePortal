using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartBranchBusinessSettingService : ISparePartBranchBusinessSettingService
    {
        private readonly ISparePartBranchBusinessSettingRepo _repo;
        public SparePartBranchBusinessSettingService(ISparePartBranchBusinessSettingRepo repo)
        {
            _repo = repo;
        }

        public async Task<SparePartBranchBusinessSetting> AddBranchBusinessSettingAsync(SparePartBranchBusinessSetting Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<SparePartBranchBusinessSetting> ArchiveBranchBusinessSettingAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<SparePartBranchBusinessSetting>> GetBranchBusinessSettingByBusinessIdAsync(long businessID)
        {
            return await _repo.GetAllAsync(x => x.SparePartBusinessSettingId == businessID);
        }

        public async Task<IEnumerable<SparePartBranchBusinessSetting>> GetBranchBusinessSettingByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<SparePartBranchBusinessSetting> UpdateBranchBusinessSettingAsync(SparePartBranchBusinessSetting Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
