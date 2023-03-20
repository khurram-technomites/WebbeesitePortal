using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class ItemService : IItemService
    {
        private readonly IItemRepo _repo;
        public ItemService(IItemRepo repo)
        {
            _repo = repo;
        }
        public async Task<Item> AddItemAsync(Item Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<Item> ArchiveItemAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<Item>> GetAllAsync(long restaurantId)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId , ChildObjects : "Category,ItemOptions");
        }

        public async Task<IEnumerable<Item>> GetAllGeneralItemAsync(long restaurantId)
        {
            return await _repo.GetGeneralItems(restaurantId);
        }

        public async Task<IEnumerable<Item>> GetByCategoryIdAsync(long ParentId, long RestaurantId , long MenuId)
        {
            return await _repo.GetCategoriesByMenuID(ParentId ,RestaurantId , MenuId);
        }

        public async Task<IEnumerable<Item>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id , ChildObjects : "Category,ItemOptions,ItemOptions.ItemOptionValues");
        }

        public async Task<Item> GetByName(long restaurantId , string Name)
        {
            IEnumerable<Item> item = await _repo.GetAllAsync(x => x.RestaurantId == restaurantId && x.Name == Name );
            return item.FirstOrDefault();
        }
     

        public async Task<IEnumerable<Item>> AddRangeAsync(IEnumerable<Item> List)
        {
            return await _repo.InsertRangeAsync(List);
        }

        public async Task<Item> UpdateItemAsync(Item Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<long> GetAllItemsCountAsync()
        {
            return await _repo.GetCount();
        }
        public async Task<long> GetAllItemsCountByRestaurantIDAsync(long restaurantId)
        {
            return await _repo.GetCount(x => x.RestaurantId == restaurantId);
        }
    }
}
