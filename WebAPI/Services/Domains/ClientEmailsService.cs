using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
namespace WebAPI.Services.Domains
{
    public class ClientEmailsService: IClientEmailsService
    {
        private readonly IClientEmailsRepo _repo;
        public ClientEmailsService(IClientEmailsRepo repo)
        {
            _repo = repo;
        }
        public Task<ClientEmails> AddClientEmailsAsync(ClientEmails Entity)
        {
            return _repo.InsertAsync(Entity);
        }
        public Task<IEnumerable<ClientEmails>> AddClientEmailsRangeAsync(IEnumerable<ClientEmails> Entity)
        {
            return _repo.InsertRangeAsync(Entity);
        }

        public Task ArchiveClientEmailsAsync(long Id)
        {
            return _repo.DeleteAsync(Id);
        }

        public Task<IEnumerable<ClientEmails>> GetAllClientEmailsAsync()
        {
            return _repo.GetAllAsync(OrderExp: x => x.Id, ChildObjects: "Garage");
        }

        public Task<IEnumerable<ClientEmails>> GetClientEmailsByIdAsync(long Id)
        {
            return _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Garage");
        }
        public Task<IEnumerable<ClientEmails>> GetClientEmailsByClientIdAsync(long ClientId)
        {
            return _repo.GetByIdAsync(x => x.ClientId == ClientId, ChildObjects: "Garage");
        }
    }
}
