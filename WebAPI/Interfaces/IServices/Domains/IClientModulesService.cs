using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IClientModulesService
    {
        Task<IEnumerable<ClientModules>> GetAllClientModuleAsync();
        Task<IEnumerable<ClientModules>> GetClientModuleByIdAsync(long Id);
        Task<IEnumerable<ClientModules>> GetClientModuleByModuleIdAsync(long Id, long ClientId);
        Task<IEnumerable<ClientModules>> GetModuleByGarageID(long GarageId);
        Task<IEnumerable<ClientModules>> GetClientModuleByClientIdAsync(long ClientId);
        Task<ClientModules> AddClientModuleAsync(ClientModules Entity);
        Task<IEnumerable<ClientModules>> AddClientModuleRangeAsync(IEnumerable<ClientModules> Entity);
        Task<IEnumerable<ClientModules>> UpdateClientModuleRangeAsync(IEnumerable<ClientModules> Entity);
        Task ArchiveClientModuleAsync(long Id);
    }
}
