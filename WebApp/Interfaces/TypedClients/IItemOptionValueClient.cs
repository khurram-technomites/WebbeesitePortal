using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IItemOptionValueClient
    {
        Task<IEnumerable<ItemOptionValueDTO>> GetAllItemOptionValuesAsync(long ItemOptionId);
        Task<ItemOptionValueDTO> GetItemOptionValueByIdAsync(long ItemOptionValueId);
        Task<ItemOptionValueDTO> AddItemOptionValueAsync(ItemOptionValueDTO Entity);
        Task<ItemOptionValueDTO> UpdateItemOptionValueAsync(ItemOptionValueDTO Entity);
        Task DeleteItemOptionValueAsync(long ItemOptionValueId);
        Task<ItemOptionValueDTO> GetByNameAsync(long ItemOptionId, string Name);
    }
}
