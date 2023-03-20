using HelperClasses.DTOs.SparePartCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ISparePartBusinessSettingClient
    {
        Task<IEnumerable<SparePartBusinessSettingDTO>> GetBusinessSettings(long SparePartId);
        Task<SparePartBusinessSettingDTO> Create(SparePartBusinessSettingDTO model);
        Task<SparePartBusinessSettingDTO> Update(SparePartBusinessSettingDTO model);

    }
}
