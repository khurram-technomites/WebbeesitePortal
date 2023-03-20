using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;
using System.Linq;


namespace WebAPI.Services.Domains
{
    public class ItemOptionValueService : IItemOptionValueService
    {
        private readonly IItemOptionValueRepo _repo;
        public ItemOptionValueService(IItemOptionValueRepo repo)
        {
            _repo = repo;
        }
        public async Task<ItemOptionValue> AddItemOptionValueAsync(ItemOptionValue Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<ItemOptionValue> ArchiveItemOptionValueAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<ItemOptionValue>> GetAllAsync(long itemOptionId)
        {
            return await _repo.GetAllAsync(x => x.ItemOptionId == itemOptionId);
        }


        public async Task<IEnumerable<ItemOptionValue>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<ItemOptionValue> GetByName(long ItemOptionId , string Name)
        {
            IEnumerable<ItemOptionValue> value =  await _repo.GetAllAsync(x => x.ItemOptionId == ItemOptionId && x.Value == Name);
            return value.FirstOrDefault();
        }

        public async Task<ItemOptionValue> UpdateItemOptionValueAsync(ItemOptionValue Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
