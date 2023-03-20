using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageExpertiseService: IGarageExpertiseService
    {
        private readonly IGarageExpertiseRepo _repo;
        public GarageExpertiseService(IGarageExpertiseRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<GarageExpertise>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageExpertise>> GetGarageExpertiseByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GarageExpertise>> GetGarageExpertiseByGarageExpertiseManagementIdAsync(long GarageExpertiseManagementId)
        {
            return await _repo.GetByIdAsync(x => x.GarageExpertiseManagementId == GarageExpertiseManagementId, ChildObjects: "Expertise,GarageExpertiseManagement");

        }

        public async Task<GarageExpertise> AddGarageExpertiseAsync(GarageExpertise Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageExpertise> UpdateGarageExpertiseAsync(GarageExpertise Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GarageExpertise> ArchiveGarageExpertiseAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task DeleteGarageExpertiseAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }
    }
}
