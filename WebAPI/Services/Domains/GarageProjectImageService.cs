using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageProjectImageService : IGarageProjectImageService
    {
        private readonly IGarageProjectImageRepo _repo;

        public GarageProjectImageService(IGarageProjectImageRepo repo)
        {
            _repo = repo;
        }

        public async Task<GarageProjectImages> AddImage(GarageProjectImages Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task DeleteImage(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<GarageProjectImages>> GetById(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GarageProjectImages>> GetByPath(string Path)
        {
            return await _repo.GetByIdAsync(x => x.ImagePath == Path);
        }

        public async Task<IEnumerable<GarageProjectImages>> GetByProjectId(long ProjectId)
        {
            return await _repo.GetByIdAsync(x => x.GarageProjectId == ProjectId);
        }
    }
}
