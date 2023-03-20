using HelperClasses.DTOs.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepo _repo;

        public MenuItemService(IMenuItemRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MenuItem>> GetById(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "MenuItemOptions,MenuItemOptions.MenuItemOptionValues,Item");
        }

        public async Task<bool> CheckMainPrice(long Id)
        {
            return  await _repo.CheckMainPrice(Id);
        }

        public async Task<IEnumerable<MenuItemByMenuDTO>> GetMenuItemByMenuId(long MenuId)
        {
            return await _repo.GetMenuItemByMenuId(MenuId);
        }
        public async Task<MenuItem> AddMenuItemAsync(MenuItem Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<MenuItem> ArchiveMenuItemAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync(long MenuId)
        {
            return await _repo.GetAllAsync(x => x.MenuId == MenuId, ChildObjects: "MenuItemOptions,MenuItemOptions.MenuItemOptionValues");
        }

        public async Task<IEnumerable<MenuItem>> GetAllCategoryByMenuAsync(long MenuId , long CategoryId)
        {
            return await _repo.GetAllAsync(x => x.MenuId == MenuId && x.CategoryId == CategoryId);
        }

        public async Task<MenuItem> UpdateMenuItemAsync(MenuItem Model)
        {
            return await _repo.UpdateAsync(Model);
        }

        public async Task<IEnumerable<MenuItem>> GetAllByCategoryAndMenuAsync(long CategoryId, long MenuId)
        {
            return await _repo.GetByIdAsync(x => x.CategoryId == CategoryId && x.MenuId == MenuId);
        }

        public async Task<IEnumerable<MenuItem>> UpdateMenuItemAsync(IEnumerable<MenuItem> Model)
        {
            return await _repo.UpdateRangeAsync(Model);
        }

        public async Task<IEnumerable<MenuItem>> GetByItemAsync(long ItemId)
        {
            return await _repo.GetByIdAsync(x => x.ItemId == ItemId);
        }
    }
}
