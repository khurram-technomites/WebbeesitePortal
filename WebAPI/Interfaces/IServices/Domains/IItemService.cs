using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IItemService
    {
        Task<long> GetAllItemsCountAsync();
        Task<long> GetAllItemsCountByRestaurantIDAsync(long restaurantId);
        Task<IEnumerable<Item>> GetAllAsync(long restaurantId);
        Task<IEnumerable<Item>> GetByIdAsync(long Id);
        Task<IEnumerable<Item>> GetByCategoryIdAsync(long ParentId, long RestaurantId, long MenuId);
        Task<Item> AddItemAsync(Item Model);
        Task<Item> UpdateItemAsync(Item Model);
        Task<IEnumerable<Item>> GetAllGeneralItemAsync(long restaurantId);
        Task<Item> ArchiveItemAsync(long Id);
        Task<Item> GetByName(long restaurantId, string Name);
        Task<IEnumerable<Item>> AddRangeAsync(IEnumerable<Item> List);
    }
}
