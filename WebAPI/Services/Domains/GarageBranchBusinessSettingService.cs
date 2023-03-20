using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageBranchBusinessSettingService : IGarageBranchBusinessSettingService
    {
        private readonly IGarageBranchBusinessSettingRepo _repo;
        public GarageBranchBusinessSettingService(IGarageBranchBusinessSettingRepo repo)
        {
            _repo = repo;
        }

        public async Task<GarageBranchBusinessSetting> AddBranchBusinessSettingAsync(GarageBranchBusinessSetting Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<GarageBranchBusinessSetting> ArchiveBranchBusinessSettingAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<GarageBranchBusinessSetting>> GetBranchBusinessSettingByBusinessIdAsync(long businessID)
        {
            return await _repo.GetAllAsync(x => x.GarageBusinessSettingId == businessID);
        }

        public async Task<IEnumerable<GarageBranchBusinessSetting>> GetBranchBusinessSettingByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<GarageBranchBusinessSetting> UpdateBranchBusinessSettingAsync(GarageBranchBusinessSetting Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
