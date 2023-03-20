using HelperClasses.DTOs.GarageCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageBranchBusinessSettingClient
    {
        Task<IEnumerable<GarageBranchBusinessSettingDTO>> GetBusinessSettings(long garageId);
        Task<GarageBranchBusinessSettingDTO> Create(GarageBranchBusinessSettingDTO model);
        Task<GarageBranchBusinessSettingDTO> Update(GarageBranchBusinessSettingDTO model);
        Task<GarageBranchBusinessSettingDTO> GetByIdAsync(long Id);
        Task ArchiveBranchBusinessSetting(long Id);
    }
}
