using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantTransactionHistoryService
    {
        Task<IEnumerable<RestaurantTransactionHistory>> GetAllAsync();
        Task<IEnumerable<RestaurantTransactionHistory>> GetByRestaurantTransactionHistoryIdAsync(long UserId);
        Task<IEnumerable<RestaurantTransactionHistory>> GetRestaurantTransactionHistoryByRestaurantIdAsync(long RestaurentId);
        Task<RestaurantTransactionHistory> AddRestaurantTransactionHistoryAsync(RestaurantTransactionHistory Model);
        Task<RestaurantTransactionHistory> UpdateRestaurantTransactionHistoryAsync(RestaurantTransactionHistory Model);
        Task<RestaurantTransactionHistory> ArchiveRestaurantTransactionHistoryAsync(long Id);

    }
}
