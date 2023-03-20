using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
namespace WebAPI.Services.Domains
{
    public class ClientDomainSuggestionsService: IClientDomainSuggestionsService
    {
        private readonly IClientDomainSuggestionsRepo _repo;
        public ClientDomainSuggestionsService(IClientDomainSuggestionsRepo repo)
        {
            _repo = repo;
        }
        public Task<ClientDomainSuggestions> AddClientDomainAsync(ClientDomainSuggestions Entity)
        {
            return _repo.InsertAsync(Entity);
        }
        public Task<IEnumerable<ClientDomainSuggestions>> AddClientDomainRangeAsync(IEnumerable<ClientDomainSuggestions> Entity)
        {
            return _repo.InsertRangeAsync(Entity);
        }

        public Task ArchiveClientDomainAsync(long Id)
        {
            return _repo.DeleteAsync(Id);
        }

        public Task<IEnumerable<ClientDomainSuggestions>> GetAllClientDomainAsync()
        {
            return _repo.GetAllAsync(OrderExp: x => x.Id, ChildObjects: "Garage");
        }

        public Task<IEnumerable<ClientDomainSuggestions>> GetClientDomainByIdAsync(long Id)
        {
            return _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Garage");
        }
        public Task<IEnumerable<ClientDomainSuggestions>> GetClientDomainByClientIdAsync(long ClientId)
        {
            return _repo.GetByIdAsync(x => x.ClientId == ClientId, ChildObjects: "Garage");
        }
    }
}
