using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IMenuItemOptionValueService
    {
        Task<IEnumerable<MenuItemOptionValue>> GetAllAsync(long MenuItemOptionId);
        Task<IEnumerable<MenuItemOptionValue>> GetByIdAsync(long Id);
        Task<MenuItemOptionValue> AddMenuItemOptionValueAsync(MenuItemOptionValue Model);
        Task<MenuItemOptionValue> UpdateMenuItemOptionValueAsync(MenuItemOptionValue Model);
        Task<MenuItemOptionValue> ArchiveMenuItemOptionValueAsync(long Id);
        Task DeleteMenuItemOptionValue(long Id);
    }
}
