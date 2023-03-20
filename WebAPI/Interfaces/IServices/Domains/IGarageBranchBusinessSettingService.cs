using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageBranchBusinessSettingService
    {
        Task<IEnumerable<GarageBranchBusinessSetting>> GetBranchBusinessSettingByBusinessIdAsync(long businessId);
        Task<IEnumerable<GarageBranchBusinessSetting>> GetBranchBusinessSettingByIdAsync(long Id);
        Task<GarageBranchBusinessSetting> AddBranchBusinessSettingAsync(GarageBranchBusinessSetting Entity);
        Task<GarageBranchBusinessSetting> UpdateBranchBusinessSettingAsync(GarageBranchBusinessSetting Entity);
        Task<GarageBranchBusinessSetting> ArchiveBranchBusinessSettingAsync(long Id);
    }
}
