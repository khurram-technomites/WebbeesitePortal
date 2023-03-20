using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageTeamManagementService: IGarageTeamManagementService
    {
        private readonly IGarageTeamManagementRepo _repo;
        public GarageTeamManagementService(IGarageTeamManagementRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GarageTeamManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageTeamManagement>> GetGarageTeamManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GarageTeamManagement>> GetGarageTeamManagementByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }
         public async Task<long> GetGarageTeamManagementCountByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetCount(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }

        public async Task<GarageTeamManagement> AddGarageTeamManagementAsync(GarageTeamManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageTeamManagement> UpdateGarageTeamManagementAsync(GarageTeamManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GarageTeamManagement> ArchiveGarageTeamManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
