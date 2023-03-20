using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageContentManagementService : IGarageContentManagementService
    {
        private readonly IGarageContentManagementRepo _repo;

        public GarageContentManagementService(IGarageContentManagementRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<GarageContentManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageContentManagement>> GetGarageContentManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Garage");
        }

        public async Task<IEnumerable<GarageContentManagement>> GetGarageContentManagementByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }

        public async Task<GarageContentManagement> AddGarageContentManagementAsync(GarageContentManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageContentManagement> UpdateGarageContentManagementAsync(GarageContentManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GarageContentManagement> ArchiveGarageContentManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
