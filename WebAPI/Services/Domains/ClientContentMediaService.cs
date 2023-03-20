using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
namespace WebAPI.Services.Domains
{
    public class ClientContentMediaService: IClientContentMediaService
    {
        private readonly IClientContentMediaRepo _repo;
        public ClientContentMediaService(IClientContentMediaRepo repo)
        {
            _repo = repo;
        }
        public Task<ClientContentMedia> AddClientContentMediaAsync(ClientContentMedia Entity)
        {
            return _repo.InsertAsync(Entity);
        }
        public Task<IEnumerable<ClientContentMedia>> AddClientContentMediaRangeAsync(IEnumerable<ClientContentMedia> Entity)
        {
            return _repo.InsertRangeAsync(Entity);
        }

        public Task ArchiveClientContentMediaAsync(long Id)
        {
            return _repo.DeleteAsync(Id);
        }

        public Task<IEnumerable<ClientContentMedia>> GetAllClientContentMediaAsync()
        {
            return _repo.GetAllAsync(OrderExp: x => x.Id, ChildObjects: "Garage");
        }

        public Task<IEnumerable<ClientContentMedia>> GetClientContentMediaByIdAsync(long Id)
        {
            return _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Garage");
        }
        public Task<IEnumerable<ClientContentMedia>> GetClientContentMediaByClientIdAsync(long ClienId)
        {
            return _repo.GetByIdAsync(x => x.ClientId == ClienId, ChildObjects: "Garage");
        }
    }
}
