using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IModuleClient
    {
        Task<IEnumerable<ModuleDTO>> GetAllAsync();
        Task<IEnumerable<ModuleDTO>> GetModuleById(long Id);
      
        Task<ModuleDTO> AddModuleAsync(ModuleDTO Entity);
        Task<ModuleDTO> ToggleActiveStatus(long Id);
        Task<ModuleDTO> UpdateModuleAsync(ModuleDTO Entity);
        Task DeleteModuleAsync(long Id);
    }
}
