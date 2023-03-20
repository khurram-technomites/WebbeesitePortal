using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IIntegrationSettingClient
    {
        Task<IEnumerable<IntegrationSettingDTO>> GetIntegrationSettings();
        Task<IntegrationSettingDTO> Create(IntegrationSettingDTO model);
        Task<IntegrationSettingDTO> Update(IntegrationSettingDTO model);
    }
}
