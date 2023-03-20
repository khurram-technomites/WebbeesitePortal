using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GaragePartnersManagementService: IGaragePartnersManagementService
    {
        private readonly IGaragePartnersManagementRepo _repo;
        public GaragePartnersManagementService(IGaragePartnersManagementRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<GaragePartnersManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<long> GetAllPartnersByGarageIdAsync(long garageId)
        {
            return await _repo.GetCount(x => x.GarageId == garageId);
        }

        public async Task<long> GetPositionCount(long garageId)
        {
            IEnumerable<GaragePartnersManagement> result = await _repo.GetByIdAsync(x => x.GarageId == garageId);
            if (result.Any())
            {
                return result.DefaultIfEmpty().Max(x => x.Position);

            }
            return 0;
        }

        public async Task<IEnumerable<GaragePartnersManagement>> GetGaragePartnersManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GaragePartnersManagement>> GetGaragePartnersManagementByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }
        public async Task<long> GetGaragePartnersManagementCountByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetCount(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }
        public async Task<GaragePartnersManagement> AddGaragePartnersManagementAsync(GaragePartnersManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GaragePartnersManagement> UpdateGaragePartnersManagementAsync(GaragePartnersManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GaragePartnersManagement> ArchiveGaragePartnersManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
