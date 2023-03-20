using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageTestimonialsService: IGarageTestimonialsService
    {
        private readonly IGarageTestimonialsRepo _repo;
        public GarageTestimonialsService(IGarageTestimonialsRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GarageTestimonials>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageTestimonials>> GetGarageTestimonialsByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<long> GetCountByGarageIdAsync(long garageId)
        {
            return await _repo.GetCount(x => x.GarageId == garageId);
        }

        public async Task<IEnumerable<GarageTestimonials>> GetGarageTestimonialsByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }

        public async Task<GarageTestimonials> AddGarageTestimonialsAsync(GarageTestimonials Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageTestimonials> UpdateGarageTestimonialsAsync(GarageTestimonials Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GarageTestimonials> ArchiveGarageTestimonialsAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
