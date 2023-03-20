using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantTransactionHistoryService : IRestaurantTransactionHistoryService
    {
        private readonly IRestaurantTransactionHistoryRepo _repo;
        public RestaurantTransactionHistoryService(IRestaurantTransactionHistoryRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<RestaurantTransactionHistory>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<RestaurantTransactionHistory> AddRestaurantTransactionHistoryAsync(RestaurantTransactionHistory Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<IEnumerable<RestaurantTransactionHistory>> GetRestaurantTransactionHistoryByRestaurantIdAsync(long RestaurentId)
        {
            return await _repo.GetByIdAsync(x => x.Restaurant.Id == RestaurentId);
        }

        public async Task<IEnumerable<RestaurantTransactionHistory>> GetByRestaurantTransactionHistoryIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<RestaurantTransactionHistory> ArchiveRestaurantTransactionHistoryAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<RestaurantTransactionHistory> UpdateRestaurantTransactionHistoryAsync(RestaurantTransactionHistory Model)
        {
            return await _repo.UpdateAsync(Model);
        }

    }
}
