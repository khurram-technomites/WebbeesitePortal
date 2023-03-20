using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IItemClient
    {
        Task<IEnumerable<ItemDTO>> GetAllItemsAsync(long RestaurantId);
        Task<ItemDTO> GetItemByIdAsync(long ItemId);
        Task<ItemDTO> GetItemByCategoryIdAsync(long CategoryId, long RestaurantId);
        Task<ItemDTO> AddItemAsync(ItemDTO Entity);
        Task<ItemDTO> UpdateItemAsync(ItemDTO Entity);
        Task<IEnumerable<ItemDTO>> AddItemRangeAsync(IEnumerable<ItemDTO> Entity);
        Task DeleteItemAsync(long ItemId);
        Task<IEnumerable<ItemDTO>> GetAllGeneralAsync(long restaurantId);
        Task<ItemDTO> GetByNameAsync(long RestaurantId, string Name);
        Task<ItemDTO> ToggleActiveStatus(long ItemId);
        Task<IEnumerable<ItemDTO>> GetItemsByCategoryIdAsync(long CategoryId, long RestaurantId , long MenuId);
    }
}
