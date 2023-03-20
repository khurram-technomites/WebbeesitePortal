using HelperClasses.DTOs.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IMenuItemClient
    {
        Task<IEnumerable<MenuItemDTO>> GetAllMenuItemsAsync(long MenuId);
        Task<MenuItemDTO> GetMenuItemByIdAsync(long MenuItemId);
        Task<MenuItemDTO> AddMenuItemAsync(MenuItemDTO Entity);
        Task<MenuItemDTO> UpdateMenuItemAsync(MenuItemDTO Entity);
        Task DeleteMenuItemAsync(long MenuItemId);
        Task<MenuItemDTO> ToggleActiveStatus(long MenuItemId);
        Task<IEnumerable<MenuItemByMenuDTO>> GetAllMenuItemsByMenuAsync(long MenuId);
        Task<bool> CheckMainPrice(long Id);
        Task<MenuItemDTO> SavePosition(MenuItemDTO Entity);
        Task<MenuItemDTO> SaveCategoryPosition(MenuItemDTO Entity);
    }
}
