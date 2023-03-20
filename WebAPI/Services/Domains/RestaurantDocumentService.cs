using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantDocumentService : IRestaurantDocumentService
    {
        private readonly IRestaurantDocumentRepo _repo;

        public RestaurantDocumentService(IRestaurantDocumentRepo repo)
        {
            _repo = repo;
        }
        public async Task<RestaurantDocument> AddRestaurantDocumentAsync(RestaurantDocument Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task DeleteRestaurantDocumentAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<RestaurantDocument>> GetAllByRestaurantAsync(long RestaurantId)
        {
            return await _repo.GetByIdAsync(x => x.ResturantId == RestaurantId);
        }

        public async Task<IEnumerable<RestaurantDocument>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<RestaurantDocument> UpdateRestaurantDocumentAsync(RestaurantDocument Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
