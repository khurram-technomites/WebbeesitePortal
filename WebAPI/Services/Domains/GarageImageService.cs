using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageImageService : IGarageImageService
    {
        private readonly IGarageImageRepo _repo;
        public GarageImageService(IGarageImageRepo repo)
        {
            _repo = repo;
        }

        public async Task DeleteGarageImage(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<GarageImage>> GetGarageImagesByImagePath(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Image == Path);
        }

        public async Task<IEnumerable<GarageImage>> GetImagesByGarage(long GarageId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GarageId);
        }
    }
}
