using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface ICategoryRepo : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesByMenuID(long MenuId, long restaurantId);
        Task<IEnumerable<Category>> GetGeneralCategories(long restaurantId);
        Task<List<CategoryDTO>> GetByRestaurantBranchId(long restaurantBranchId);
    }
}
