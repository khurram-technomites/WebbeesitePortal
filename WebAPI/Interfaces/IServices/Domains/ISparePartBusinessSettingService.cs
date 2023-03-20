using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartBusinessSettingService 
    {
        Task<IEnumerable<SparePartBusinessSetting>> GetBusinessSettingBySparePartIdAsync(long SparePartId);
        Task<IEnumerable<SparePartBusinessSetting>> GetBusinessSettingByIdAsync(long Id);
        Task<SparePartBusinessSetting> AddBusinessSettingAsync(SparePartBusinessSetting Entity);
        Task<SparePartBusinessSetting> UpdateBusinessSettingAsync(SparePartBusinessSetting Entity);
        Task<SparePartBusinessSetting> ArchiveBusinessSettingAsync(long Id);
    }
}
