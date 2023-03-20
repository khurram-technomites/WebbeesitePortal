using HelperClasses.DTOs.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IMenuItemOptionValueClient
    {
        Task<IEnumerable<MenuItemOptionValueDTO>> GetAllMenuItemOptionValuesAsync(long MeneItemOptionId);
        Task<MenuItemOptionValueDTO> GetMenuItemOptionValueByIdAsync(long MenuItemOptionValueId);
        Task<MenuItemOptionValueDTO> AddMenuItemOptionValueAsync(MenuItemOptionValueDTO Entity);
        Task<MenuItemOptionValueDTO> UpdateMenuItemOptionValueAsync(MenuItemOptionValueDTO Entity);
        Task DeleteMenuItemOptionValueAsync(long MenuItemOptionValueId);
        Task Delete(long MenuItemOptionValueId);
    }
}
