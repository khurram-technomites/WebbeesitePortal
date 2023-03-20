using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class MenuItemOptionService : IMenuItemOptionService
    {
        private readonly IMenuItemOptionRepo _repo;

        public MenuItemOptionService(IMenuItemOptionRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<MenuItemOption>> GetById(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "MenuItemOptionValues");
        }

        public async Task<MenuItemOption> AddMenuItemOptionAsync(MenuItemOption Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<MenuItemOption> ArchiveMenuItemOptionAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task DeleteMenuItemOption(long Id)
        {
            await _repo.DeleteAsync(Id);
        }


        public async Task<IEnumerable<MenuItemOption>> GetAllAsync(long MenuItemId)
        {
            return await _repo.GetAllAsync(x => x.MenuItemId == MenuItemId , ChildObjects:"MenuItemOptionValues");
        }

        public async Task<IEnumerable<MenuItemOption>> GetMainPriceAsync(long MenuItemId , long MenuItemOptionId)
        {
            var result = await _repo.GetAllAsync(x => x.MenuItemId == MenuItemId && x.IsPriceMain == true && x.Id != MenuItemOptionId);
            if (result.Any())
            {
                var list = await _repo.GetAllAsync(x => x.MenuItemId == MenuItemId);
                foreach (var item in list)
                {
                    item.IsPriceMain = false;
                    await _repo.UpdateAsync(item);
                }

            }

            return result;
        }

        public async Task<MenuItemOption> UpdateMenuItemOptionAsync(MenuItemOption Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
