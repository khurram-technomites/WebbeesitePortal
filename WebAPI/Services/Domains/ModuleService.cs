using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
namespace WebAPI.Services.Domains
{
    public class ModuleService:IModuleService
    {
        private readonly IModuleRepo _repo;

        public ModuleService(IModuleRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Module>> GetAllAsync()
        {
            return await _repo.GetAllAsync(x=>x.ArchivedDate== null);
        }
        public async Task<IEnumerable<Module>> GetModuleByName(string Name)
        {
            return await _repo.GetAllAsync(x => x.ServiceName == Name);
        }

        public async Task<IEnumerable<Module>> GetModuleById(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<Module> AddModuleAsync(Module Entity)
        {
            return await _repo.InsertAsync(Entity);
        }
        public async Task<Module> UpdateModuleAsync(Module Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }

        public async Task<Module> ArchiveModuleAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
