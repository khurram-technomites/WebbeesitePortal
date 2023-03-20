using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageBusinessSettingService 
    {
        Task<IEnumerable<GarageBusinessSetting>> GetBusinessSettingByGarageIdAsync(long garageId);
        Task<IEnumerable<GarageBusinessSetting>> GetBusinessSettingByIdAsync(long Id);
        Task<IEnumerable<GarageBusinessSetting>> GetBusinessSettingByGarageId(long Id);
        Task<GarageBusinessSetting> AddBusinessSettingAsync(GarageBusinessSetting Entity);
        Task<GarageBusinessSetting> UpdateBusinessSettingAsync(GarageBusinessSetting Entity);
        Task<GarageBusinessSetting> ArchiveBusinessSettingAsync(long Id);
    }
}
