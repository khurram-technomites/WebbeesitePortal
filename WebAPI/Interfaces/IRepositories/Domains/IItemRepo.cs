using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface IItemRepo : IRepository<Item>
    {
        Task<IEnumerable<Item>> GetCategoriesByMenuID(long categoryId, long restaurantId, long menuId);
        Task<IEnumerable<Item>> GetGeneralItems(long restaurantId);
    }
}
