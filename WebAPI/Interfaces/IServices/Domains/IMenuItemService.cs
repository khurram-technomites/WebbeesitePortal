using HelperClasses.DTOs.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItem>> GetById(long Id);
        Task<IEnumerable<MenuItem>> GetAllAsync(long MenuId);
        Task<IEnumerable<MenuItem>> GetByItemAsync(long ItemId);
        Task<IEnumerable<MenuItem>> GetAllByCategoryAndMenuAsync(long CategoryId, long MenuId);
        Task<MenuItem> AddMenuItemAsync(MenuItem Model);
        Task<MenuItem> UpdateMenuItemAsync(MenuItem Model);
        Task<IEnumerable<MenuItem>> UpdateMenuItemAsync(IEnumerable<MenuItem> Model);
        Task<MenuItem> ArchiveMenuItemAsync(long Id);
        Task<IEnumerable<MenuItemByMenuDTO>> GetMenuItemByMenuId(long MenuId);
        Task<bool> CheckMainPrice(long Id);
        Task<IEnumerable<MenuItem>> GetAllCategoryByMenuAsync(long MenuId, long CategoryId);
    }
}
