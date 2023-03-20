using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class ItemOptionService : IItemOptionService
    {
        private readonly IItemOptionRepo _repo;
        public ItemOptionService(IItemOptionRepo repo)
        {
            _repo = repo;
        }
        public async Task<ItemOption> AddItemOptionAsync(ItemOption Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<ItemOption> ArchiveItemOptionAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<ItemOption>> GetAllAsync(long itemId)
        {
            return await _repo.GetAllAsync(x => x.ItemId == itemId);
        }

        public async Task<IEnumerable<ItemOption>> GetMainPriceAsync(long ItemId, long ItemOptionId)
        {
            var result = await _repo.GetAllAsync(x => x.ItemId == ItemId && x.IsPriceMain == true && x.Id != ItemOptionId);
            if (result.Any())
            {
                var list = await _repo.GetAllAsync(x => x.ItemId == ItemId);
                foreach (var item in list)
                {
                    item.IsPriceMain = false;
                    await _repo.UpdateAsync(item);
                }

            }

            return result;
        }
        public async Task<IEnumerable<ItemOption>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id , ChildObjects: "ItemOptionValues");
        }

        public async Task<ItemOption> GetByName(long ItemId, string Name)
        {

            IEnumerable<ItemOption> itemOption = await _repo.GetAllAsync(x => x.ItemId == ItemId && x.Title == Name);
            return itemOption.FirstOrDefault();

        }

        public async Task<IEnumerable<ItemOption>> GetByItemIdAsync(long ItemId)
        {
            return await _repo.GetByIdAsync(x => x.ItemId == ItemId , ChildObjects: "ItemOptionValues");
        }

        public async Task<ItemOption> UpdateItemOptionAsync(ItemOption Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
