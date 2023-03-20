using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class ExpertiseService: IExpertiseService
    {
        private readonly IExpertiseRepo _repo;
        public ExpertiseService(IExpertiseRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Expertise>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<Expertise>> GetExpertiseByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<Expertise> AddExpertiseAsync(Expertise Entity)
        {
            return await _repo.InsertAsync(Entity);
        }
        public async Task<Expertise> UpdateExpertiseAsync(Expertise Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }

        public async Task<Expertise> ArchiveExpertiseAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

    }
}
