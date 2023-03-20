using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartTestimonialService : ISparePartTestimonialService
    {
        private readonly ISparePartTestimonialRepo _repo;
        public SparePartTestimonialService(ISparePartTestimonialRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartTestimonial>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SparePartTestimonial>> GetSparePartTestimonialByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<long> GetCountBySparePartDealerIdAsync(long SparePartDealerId)
        {
            return await _repo.GetCount(x => x.SparePartDealerId == SparePartDealerId);
        }

        public async Task<IEnumerable<SparePartTestimonial>> GetSparePartTestimonialBySparePartDealerIdAsync(long SparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == SparePartDealerId, ChildObjects: "SparePartDealer");
        }

        public async Task<SparePartTestimonial> AddSparePartTestimonialAsync(SparePartTestimonial Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartTestimonial> UpdateSparePartTestimonialAsync(SparePartTestimonial Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartTestimonial> ArchiveSparePartTestimonialAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
