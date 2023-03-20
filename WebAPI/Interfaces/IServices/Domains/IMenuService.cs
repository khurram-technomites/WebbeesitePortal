using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IMenuService
    {
        Task<long> GetAllMenusCountAsync(long RestaurantId);
        Task<IEnumerable<Menu>> GetAllAsync(long restaurantBranchId);
        Task<IEnumerable<Menu>> GetByIdAsync(long Id);
        Task<Menu> AddMenuAsync(Menu Model);
        Task<Menu> UpdateMenuAsync(Menu Model);
        Task<Menu> ArchiveMenuAsync(long Id);
        Task<IEnumerable<Menu>> GetGeneralMenuAsync();
        Task<IEnumerable<Menu>> GetAllByBranchIdAsync(long branchId, long id);
    }
}
