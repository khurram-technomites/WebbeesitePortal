using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IIntegrationSettingService
    {
        Task<IEnumerable<IntegrationSetting>> GetAllAsync();
        Task<IEnumerable<IntegrationSetting>> GetIntegrationSettingByIdAsync(long Id);
        Task<IntegrationSetting> AddIntegrationSettingAsync(IntegrationSetting Entity);
        Task<IntegrationSetting> UpdateIntegrationSettingAsync(IntegrationSetting Entity);
    }
}
