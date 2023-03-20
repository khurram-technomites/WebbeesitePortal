using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartExpertiseService : ISparePartExpertiseService
    {
        private readonly ISparePartExpertiseRepo _repo;
        public SparePartExpertiseService(ISparePartExpertiseRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartExpertise>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SparePartExpertise>> GetSparePartExpertiseByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartExpertise>> GetSparePartExpertiseBySparePartExpertiseManagementIdAsync(long SparePartExpertiseManagementId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartExpertiseManagementId == SparePartExpertiseManagementId, ChildObjects: "Expertise,SparePartExpertiseManagement");

        }

        public async Task<SparePartExpertise> AddSparePartExpertiseAsync(SparePartExpertise Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartExpertise> UpdateSparePartExpertiseAsync(SparePartExpertise Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartExpertise> ArchiveSparePartExpertiseAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task DeleteSparePartExpertiseAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }
    }
}
