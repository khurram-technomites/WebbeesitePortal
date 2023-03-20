using HelperClasses.DTOs.GarageCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IGarageBusinessSettingClient
    {
        Task<IEnumerable<GarageBusinessSettingDTO>> GetBusinessSettings(long garageId);
        Task<GarageBusinessSettingDTO> GetBusinessSetting(long garageId);
        Task<GarageBusinessSettingDTO> Create(GarageBusinessSettingDTO model);
        Task<GarageBusinessSettingDTO> Update(GarageBusinessSettingDTO model);

    }
}
