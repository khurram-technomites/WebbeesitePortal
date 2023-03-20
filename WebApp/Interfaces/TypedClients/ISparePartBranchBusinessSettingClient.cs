using HelperClasses.DTOs.SparePartCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartBranchBusinessSettingClient 
    {
        Task<IEnumerable<SparePartBranchBusinessSettingDTO>> GetBusinessSettings(long SparePartId);
        Task<SparePartBranchBusinessSettingDTO> Create(SparePartBranchBusinessSettingDTO model);
        Task<SparePartBranchBusinessSettingDTO> Update(SparePartBranchBusinessSettingDTO model);
        Task<SparePartBranchBusinessSettingDTO> GetByIdAsync(long Id);
        Task ArchiveBranchBusinessSetting(long Id);
    }
}
