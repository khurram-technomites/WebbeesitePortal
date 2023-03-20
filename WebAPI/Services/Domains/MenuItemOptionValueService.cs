using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class MenuItemOptionValueService : IMenuItemOptionValueService
    {
        private readonly IMenuItemOptionValueRepo _repo;
        public MenuItemOptionValueService(IMenuItemOptionValueRepo repo)
        {
            _repo = repo;
        }
        public async Task<MenuItemOptionValue> AddMenuItemOptionValueAsync(MenuItemOptionValue Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<MenuItemOptionValue> ArchiveMenuItemOptionValueAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<MenuItemOptionValue>> GetAllAsync(long MenuItemOptionId)
        {
            return await _repo.GetAllAsync(x => x.MenuItemOptionId == MenuItemOptionId);
        }

        public async Task DeleteMenuItemOptionValue(long Id)
        {
            await _repo.DeleteAsync(Id);
        }
        public async Task<IEnumerable<MenuItemOptionValue>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<MenuItemOptionValue> UpdateMenuItemOptionValueAsync(MenuItemOptionValue Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
