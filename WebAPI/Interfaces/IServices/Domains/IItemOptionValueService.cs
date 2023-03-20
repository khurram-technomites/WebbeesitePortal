using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IItemOptionValueService
    {
        Task<IEnumerable<ItemOptionValue>> GetAllAsync(long ItemOptionId);
        Task<IEnumerable<ItemOptionValue>> GetByIdAsync(long Id);
        Task<ItemOptionValue> AddItemOptionValueAsync(ItemOptionValue Model);
        Task<ItemOptionValue> UpdateItemOptionValueAsync(ItemOptionValue Model);
        Task<ItemOptionValue> ArchiveItemOptionValueAsync(long Id);
        Task<ItemOptionValue> GetByName(long ItemOptionId, string Name);
    }
}
