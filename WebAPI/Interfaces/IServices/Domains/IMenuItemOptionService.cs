using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IMenuItemOptionService
    {
        Task<IEnumerable<MenuItemOption>> GetById(long Id);
        Task<IEnumerable<MenuItemOption>> GetAllAsync(long MenuItemOptionId);
        Task<MenuItemOption> AddMenuItemOptionAsync(MenuItemOption Model);
        Task<MenuItemOption> UpdateMenuItemOptionAsync(MenuItemOption Model);
        Task<MenuItemOption> ArchiveMenuItemOptionAsync(long Id);
        Task DeleteMenuItemOption(long Id);
        Task<IEnumerable<MenuItemOption>> GetMainPriceAsync(long MenuItemId, long MenuItemOptionId);
    }
}
