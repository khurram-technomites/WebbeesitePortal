using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartBranchBusinessSettingService
    {
        Task<IEnumerable<SparePartBranchBusinessSetting>> GetBranchBusinessSettingByBusinessIdAsync(long businessId);
        Task<IEnumerable<SparePartBranchBusinessSetting>> GetBranchBusinessSettingByIdAsync(long Id);
        Task<SparePartBranchBusinessSetting> AddBranchBusinessSettingAsync(SparePartBranchBusinessSetting Entity);
        Task<SparePartBranchBusinessSetting> UpdateBranchBusinessSettingAsync(SparePartBranchBusinessSetting Entity);
        Task<SparePartBranchBusinessSetting> ArchiveBranchBusinessSettingAsync(long Id);
    }
}
