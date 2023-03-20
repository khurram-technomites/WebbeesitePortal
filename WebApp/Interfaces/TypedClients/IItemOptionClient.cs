using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IItemOptionClient
    {
        Task<IEnumerable<ItemOptionDTO>> GetAllItemOptionsAsync(long ItemId);
        Task<ItemOptionDTO> GetItemOptionByIdAsync(long ItemOptionId);
        Task<ItemOptionDTO> AddItemOptionAsync(ItemOptionDTO Entity);
        Task<ItemOptionDTO> UpdateItemOptionAsync(ItemOptionDTO Entity);
        Task<ItemOptionDTO> GetByName(long ItemId, string Name);
        Task DeleteItemOptionAsync(long ItemOptionId);
        Task<IEnumerable<ItemOptionDTO>> GetMainPrice(long ItemId, long ItemOptionId);
    }
}
