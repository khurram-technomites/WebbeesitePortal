using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICategoryService 
    {
        Task<IEnumerable<Category>> GetAllAsync(long restaurantId = 0);
        Task<List<CategoryDTO>> GetAllAsyncByBranchId(long restaurantBranchId);
        Task<IEnumerable<Category>> GetByIdAsync(long Id);
        Task<IEnumerable<Category>> GetByParentIdAsync(long ParentId , long restaurantId);
        Task<IEnumerable<Category>> GetParentCategoriesAsync(long RestaurantId);
        Task<IEnumerable<CategoryDTO>> GetAllByMenu(long MenuId, long restaurantId);
        Task<IEnumerable<Category>> AddRangeAsync(IEnumerable<Category> List);
        Task<Category> AddCategoryAsync(Category Model);
        Task<Category> UpdateCategoryAsync(Category Model);
        Task<Category> GetIdbyNameAsync(long restaurantId, string Name);
        Task<Category> ArchiveCategoryAsync(long Id);
        Task<IEnumerable<Category>> GetGeneralCategoriesAsync(long restaurantId);
        Task<long> GetAllCategoriesCountAsync(long restaurantId);
        Task<IEnumerable<CategoryDTO>> GetAllByMenuID(long MenuId, long restaurantId);
        Task<long> GetPositionCount(long RestaurantId);
    }
}
