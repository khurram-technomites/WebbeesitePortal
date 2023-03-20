using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartRequestImagesService : ISparePartRequestImagesService
    {
        private readonly ISparePartRequestImagesRepo _repo;

        public SparePartRequestImagesService(ISparePartRequestImagesRepo repo)
        {
            _repo = repo;
        }

        public async Task DeleteAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<SparePartRequestImage>> GetRequestByImageAsync(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Image == Path);
        }
    }
}
