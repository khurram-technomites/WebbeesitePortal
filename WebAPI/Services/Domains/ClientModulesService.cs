using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
namespace WebAPI.Services.Domains
{
    public class ClientModulesService: IClientModulesService
    {
        private readonly IClientModulesRepo _repo;
        public ClientModulesService(IClientModulesRepo repo)
        {
            _repo = repo;
        }
        public Task<ClientModules> AddClientModuleAsync(ClientModules Entity)
        {
            return _repo.InsertAsync(Entity);
        }
        public Task<IEnumerable<ClientModules>> AddClientModuleRangeAsync(IEnumerable<ClientModules> Entity)
        {
            return _repo.InsertRangeAsync(Entity);
        }
        public Task<IEnumerable<ClientModules>> UpdateClientModuleRangeAsync(IEnumerable<ClientModules> Entity)
        {
            return _repo.UpdateRangeAsync(Entity);
        }

        public Task ArchiveClientModuleAsync(long Id)
        {
            return _repo.DeleteAsync(Id);
        }

        public Task<IEnumerable<ClientModules>> GetAllClientModuleAsync()
        {
            return _repo.GetAllAsync(OrderExp: x => x.Id, ChildObjects: "Garage");
        }

        public Task<IEnumerable<ClientModules>> GetClientModuleByIdAsync(long Id)
        {
            return _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Garage");
        }
        public Task<IEnumerable<ClientModules>> GetModuleByGarageID(long Id)
        {
            return _repo.GetByIdAsync(x => x.Garage.Id == Id, ChildObjects: "Garage, Module");
        }
        public Task<IEnumerable<ClientModules>> GetClientModuleByModuleIdAsync(long Id,long ClientId)
        {
            return _repo.GetByIdAsync(x => x.ModuleId == Id && x.ClientId == ClientId, ChildObjects: "Garage, Module");
        }
        public Task<IEnumerable<ClientModules>> GetClientModuleByClientIdAsync(long ClientId)
        {
            return _repo.GetByIdAsync(x => x.ClientId == ClientId, ChildObjects: "Garage , Module");
        }
    }
}
