using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartCareerService : ISparePartCareerService
    {
        private readonly ISparePartCareerRepo _repo;
        public SparePartCareerService(ISparePartCareerRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartCareer>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SparePartCareer>> GetSparePartCareerByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartCareer>> GetSparePartCareerBySparePartDealerIdAsync(long sparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == sparePartDealerId, ChildObjects: "SparePartDealer");
        }

        public async Task<SparePartCareer> AddSparePartCareerAsync(SparePartCareer Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartCareer> UpdateSparePartCareerAsync(SparePartCareer Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartCareer> ArchiveSparePartCareerAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
