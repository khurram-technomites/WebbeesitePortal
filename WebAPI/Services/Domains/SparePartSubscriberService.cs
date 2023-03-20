using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartSubscriberService : ISparePartSubscriberService
    {
        private readonly ISparePartSubscriberRepo _repo;
        public SparePartSubscriberService(ISparePartSubscriberRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartSubscriber>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SparePartSubscriber>> GetSparePartSubscriberByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartSubscriber>> GetSparePartSubscriberBySparePartDealerIdAsync(long SparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == SparePartDealerId, ChildObjects: "SparePartDealer");
        }

        public async Task<SparePartSubscriber> AddSparePartSubscriberAsync(SparePartSubscriber Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartSubscriber> UpdateSparePartSubscriberAsync(SparePartSubscriber Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task ArchiveSparePartSubscriberAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<SparePartSubscriber>> GetSparePartSubscribersByEmailAsync(long Id, string Email)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == Id && x.Email == Email);
        }
    }
}
