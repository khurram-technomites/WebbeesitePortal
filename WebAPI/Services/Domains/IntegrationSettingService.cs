using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class IntegrationSettingService : IIntegrationSettingService
    {
        private readonly IIntegrationSettingRepo _repo;

        public IntegrationSettingService(IIntegrationSettingRepo repo)
        {
            _repo = repo;
        }

        public Task<IEnumerable<IntegrationSetting>> GetAllAsync()
        {
            return _repo.GetAllAsync();
        }

        public async Task<IntegrationSetting> AddIntegrationSettingAsync(IntegrationSetting Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<IEnumerable<IntegrationSetting>> GetIntegrationSettingByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IntegrationSetting> UpdateIntegrationSettingAsync(IntegrationSetting Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
