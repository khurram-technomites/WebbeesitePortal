using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartPartnersManagementService : ISparePartPartnersManagementService
    {
        private readonly ISparePartPartnersManagementRepo _repo;
        public SparePartPartnersManagementService(ISparePartPartnersManagementRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartPartnersManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<long> GetAllPartnersBySparePartDealerIdAsync(long sparePartDealerId)
        {
            return await _repo.GetCount(x => x.SparePartDealerId == sparePartDealerId);
        }

        public async Task<long> GetPositionCount(long sparePartDealerId)
        {
            IEnumerable<SparePartPartnersManagement> result = await _repo.GetByIdAsync(x => x.SparePartDealerId == sparePartDealerId);
            if (result.Any())
            {
                return result.DefaultIfEmpty().Max(x => x.Position);

            }
            return 0;
        }

        public async Task<IEnumerable<SparePartPartnersManagement>> GetSparePartPartnersManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartPartnersManagement>> GetSparePartPartnersManagementBySparePartDealerIdAsync(long sparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == sparePartDealerId, ChildObjects: "SparePartDealer");
        }

        public async Task<SparePartPartnersManagement> AddSparePartPartnersManagementAsync(SparePartPartnersManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartPartnersManagement> UpdateSparePartPartnersManagementAsync(SparePartPartnersManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartPartnersManagement> ArchiveSparePartPartnersManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
