using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartTeamManagementService : ISparePartTeamManagementService
    {
        private readonly ISparePartTeamManagementRepo _repo;
        public SparePartTeamManagementService(ISparePartTeamManagementRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartTeamManagement>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SparePartTeamManagement>> GetSparePartTeamManagementByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartTeamManagement>> GetSparePartTeamManagementBySparePartDealerIdAsync(long sparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == sparePartDealerId, ChildObjects: "SparePartDealer");
        }

        public async Task<SparePartTeamManagement> AddSparePartTeamManagementtAsync(SparePartTeamManagement Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartTeamManagement> UpdateSparePartTeamManagementAsync(SparePartTeamManagement Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartTeamManagement> ArchiveSparePartTeamManagementAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
    }
}
