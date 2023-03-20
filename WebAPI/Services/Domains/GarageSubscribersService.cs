using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageSubscribersService: IGarageSubscribersService
    {
        private readonly IGarageSubscribersRepo _repo;
        public GarageSubscribersService(IGarageSubscribersRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GarageSubscribers>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageSubscribers>> GetGarageSubscribersByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GarageSubscribers>> GetGarageSubscribersByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }

        public async Task<GarageSubscribers> AddGarageSubscribersAsync(GarageSubscribers Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageSubscribers> UpdateGarageSubscribersAsync(GarageSubscribers Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task ArchiveGarageSubscribersAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<GarageSubscribers>> GetGarageSubscribersByEmailAsync(string Email, long GarageId)
        {
            return await _repo.GetByIdAsync(x => x.Email == Email && x.GarageId == GarageId);
        }
    }
}
