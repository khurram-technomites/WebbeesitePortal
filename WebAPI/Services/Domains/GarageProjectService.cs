using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageProjectService : IGarageProjectService
    {
        private readonly IGarageProjectRepo _repo;

        public GarageProjectService(IGarageProjectRepo repo)
        {
            _repo = repo;
        }

        public async Task<GarageProject> AddProjectAsync(GarageProject Model)
        {
            return await _repo.InsertAsync(Model);
        }
        public async Task<long> GetAllProjectsByGarageIdAsync(long garageId)
        {
            return await _repo.GetCount(x => x.GarageId == garageId);
        }
        public async Task<GarageProject> ArchiveProjectAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<GarageProject>> GetByGarageAsync(long GarageId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GarageId);
        }
        public async Task<long> GetCountByGarageAsync(long GarageId)
        {
            return await _repo.GetCount(x => x.GarageId == GarageId);
        }
        public async Task<IEnumerable<GarageProject>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "GarageProjectImages");
        }

        public async Task<GarageProject> UpdateProjectAsync(GarageProject Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
