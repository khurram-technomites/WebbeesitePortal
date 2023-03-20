using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;
namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IModuleService
    {
        Task<IEnumerable<Module>> GetAllAsync();
        Task<IEnumerable<Module>> GetModuleById(long Id);
        Task<IEnumerable<Module>> GetModuleByName(string Name);
        Task<Module> AddModuleAsync(Module Model);
        Task<Module> UpdateModuleAsync(Module Model);
        Task<Module> ArchiveModuleAsync(long Id);
    }
}
