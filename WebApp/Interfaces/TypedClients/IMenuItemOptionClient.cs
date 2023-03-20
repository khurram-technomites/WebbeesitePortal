using HelperClasses.DTOs.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IMenuItemOptionClient
    {
        Task<IEnumerable<MenuItemOptionDTO>> GetAllMenuItemOptionsAsync(long ItemId);
        Task<MenuItemOptionDTO> GetMenuItemOptionByIdAsync(long MenuItemOptionId);
        Task<MenuItemOptionDTO> AddMenuItemOptionAsync(MenuItemOptionDTO Entity);
        Task<MenuItemOptionDTO> UpdateMenuItemOptionAsync(MenuItemOptionDTO Entity);
        Task DeleteMenuItemOptionAsync(long MenuItemOptionId);
        Task<IEnumerable<MenuItemOptionDTO>> GetMainPrice(long MenuItemId, long MenuItemOptionId);
        Task Delete(long MenuItemOptionId);
    }
}
