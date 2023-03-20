using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantImageService : IRestaurantImageService
    {
        private readonly IRestaurantImageRepo _repo;

        public RestaurantImageService(IRestaurantImageRepo repo)
        {
            _repo = repo;
        }
        public async Task ArchiveRestaurantImageAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }
        public async Task<IEnumerable<RestaurantImages>> GetImageByPath(string Path)
        {
            return await _repo.GetByIdAsync(x => x.Image == Path);
        }

        public async Task<IEnumerable<RestaurantImages>> GetImageByRestaurant(long Id)
        {
            return await _repo.GetByIdAsync(x => x.RestaurantId == Id);
        }
    }
}
