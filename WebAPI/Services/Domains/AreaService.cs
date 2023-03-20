using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepo _repo;
        public AreaService(IAreaRepo repo)
        {
            _repo = repo;
        }
        public Task<Areas> AddAreaAsync(Areas Entity)
        {
            return _repo.InsertAsync(Entity);
        }

        public Task<Areas> ArchiveAreaAsync(long Id)
        {
            return _repo.ArchiveAsync(Id);
        }

        public Task<IEnumerable<Areas>> GetAllAreasAsync()
        {
            return _repo.GetAllAsync(OrderExp: x => x.Id, ChildObjects: "City");
        }

        public Task<IEnumerable<Areas>> GetAreaByIdAsync(long Id)
        {
            return _repo.GetByIdAsync(x => x.Id == Id , ChildObjects: "City");
        }

        public Task<Areas> UpdateAreaAsync(Areas Entity)
        {
            return _repo.UpdateAsync(Entity);
        }
    }
}
