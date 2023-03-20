using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageServiceManagementService: IGarageServiceManagementService
    {
        private readonly IGarageServiceManagementRepo _repo;

        public GarageServiceManagementService(IGarageServiceManagementRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<GarageServiceManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageServiceManagement>> GetGarageServiceManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GarageServiceManagement>> GetGarageServiceManagementByGaragedIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        } 
        public async Task<long> GetGarageServiceManagementCountByGaragedIdAsync(long GaragedId)
        {
            return await _repo.GetCount(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }

        public async Task<GarageServiceManagement> AddGarageServiceManagementAsync(GarageServiceManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageServiceManagement> UpdateGarageServiceManagementAsync(GarageServiceManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GarageServiceManagement> ArchiveGarageServiceManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<GarageServiceManagement>> GetGarageServiceManagementBySlugAsync(string Slug)
        {
            return await _repo.GetByIdAsync(x => x.Slug == Slug);
        }
    }
}
