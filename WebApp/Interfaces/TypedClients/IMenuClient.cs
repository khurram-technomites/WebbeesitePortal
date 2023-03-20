using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IMenuClient
    {
        Task<IEnumerable<MenuDTO>> GetAllMenusAsync(long RestaurantBranchId);
        Task<MenuDTO> GetMenuByIdAsync(long MenuId);
        Task<MenuDTO> AddMenuAsync(MenuDTO Entity);
        Task<MenuDTO> UpdateMenuAsync(MenuDTO Entity);
        Task<MenuDTO> ToggleActiveStatus(long MenuId);
        Task DeleteMenuAsync(long MenuId);
        Task<MenuDTO> GetGeneralMenu();
        Task<IEnumerable<MenuDTO>> GetAllMenuByBranchIdAsync(long BranchId, long id = 0);
        Task<MenuDTO> SavePosition(MenuDTO Entity);
    }
}
