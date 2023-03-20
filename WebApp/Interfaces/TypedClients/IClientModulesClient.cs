using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelperClasses.DTOs;
using System;
namespace WebApp.Interfaces.TypedClients
{
    public interface IClientModulesClient
    {
        Task<IEnumerable<ClientModulesDTO>> GetClientModule();
        Task<IEnumerable<ClientModulesDTO>> GetClientModuleByClientId(long ClientId);
        Task<LayoutModuleDTO> GetModuleByClientId(long ClientId);
        Task<ClientModulesDTO> GetClientModuleByID(long Id);
        Task<ClientModulesDTO> Create(ClientModulesDTO model);
        Task<IEnumerable<ClientModulesDTO>> CreateRange(IEnumerable<ClientModulesDTO> model);
        Task Delete(long Id);
    }
}
