using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IItemOptionService
    {
        Task<IEnumerable<ItemOption>> GetAllAsync(long ItemOptionId);
        Task<IEnumerable<ItemOption>> GetByIdAsync(long Id);
        Task<ItemOption> AddItemOptionAsync(ItemOption Model);
        Task<ItemOption> UpdateItemOptionAsync(ItemOption Model);
        Task<ItemOption> GetByName(long ItemId, string Name);
        Task<ItemOption> ArchiveItemOptionAsync(long Id);
        Task<IEnumerable<ItemOption>> GetByItemIdAsync(long ItemId);
        Task<IEnumerable<ItemOption>> GetMainPriceAsync(long ItemId, long ItemOptionId);
    }
}
