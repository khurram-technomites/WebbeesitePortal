using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageMenuManagementService: IGarageMenuManagementService
    {
        private readonly IGarageMenuManagementRepo _repo;
        public GarageMenuManagementService(IGarageMenuManagementRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GarageMenuManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync(ChildObjects : "GarageMenu");
        }
        public async Task<IEnumerable<GarageMenuManagement>> GetGarageMenuManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id );
        }

        public async Task<IEnumerable<GarageMenuManagement>> GetGarageMenuManagementByGaragedIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage,GarageMenu");
        }

        public async Task<long> GetCountByGarageIdAsync(long garageId)
        {
            return await _repo.GetCount(x => x.GarageId == garageId);
        }

        public async Task<IEnumerable<GarageMenuManagement>> GetGarageMenuManagementByMenuIdAsync(long MenuId)
        {
            return await _repo.GetByIdAsync(x => x.GarageMenuId == MenuId, ChildObjects: "GarageMenu");
        }

        public async Task<GarageMenuManagement> AddGarageMenuManagementAsync(GarageMenuManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageMenuManagement> UpdateGarageMenuManagementAsync(GarageMenuManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GarageMenuManagement> ArchiveGarageMenuManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }


}
