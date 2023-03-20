using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageExpertiseManagementService: IGarageExpertiseManagementService
    {
        private readonly IGarageExpertiseManagementRepo _repo;
        public GarageExpertiseManagementService(IGarageExpertiseManagementRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GarageExpertiseManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageExpertiseManagement>> GetGarageExpertiseManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GarageExpertiseManagement>> GetGarageExpertiseManagementByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }
        public async Task<long> GetGarageExpertiseCountByGarageIdAsnyc(long GarageId)
        {
            return await _repo.GetCount(x => x.GarageId == GarageId, ChildObjects: "Garage");
        }
        public async Task<GarageExpertiseManagement> AddGarageExpertiseManagementAsync(GarageExpertiseManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageExpertiseManagement> UpdateGarageExpertiseManagementAsync(GarageExpertiseManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GarageExpertiseManagement> ArchiveGarageExpertiseManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task DeleteGarageExpertiseManagementAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }
    }
}
