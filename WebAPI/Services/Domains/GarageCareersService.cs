using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageCareersService: IGarageCareersService
    {
        private readonly IGarageCareersRepo _repo;
        public GarageCareersService(IGarageCareersRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GarageCareers>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageCareers>> GetGarageCareersByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GarageCareers>> GetGarageCareersByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }

        public async Task<GarageCareers> AddGarageCareersAsync(GarageCareers Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageCareers> UpdateGarageCareersAsync(GarageCareers Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GarageCareers> ArchiveGarageCareersAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
