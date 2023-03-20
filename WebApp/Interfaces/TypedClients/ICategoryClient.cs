using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ICategoryClient
    {
        Task<IEnumerable<CategoryDTO>> GetAllCategorysAsync(long RestaurantId);
        Task<CategoryDTO> GetCategoryByIdAsync(long CategoryId);
        Task<CategoryDTO> GetByName(long restaurantId, string Name);
        Task<CategoryDTO> GetCategoryByParentIdAsync(long ParentCategoryId , long RestaurantId);
        Task<IEnumerable<CategoryDTO>> GetParentCategoriesAsync(long RestaurantId);
        Task<CategoryDTO> AddCategoryAsync(CategoryDTO Entity);
        Task<IEnumerable<CategoryDTO>> AddCategoryRangeAsync(IEnumerable<CategoryDTO> Entity);
        Task<CategoryDTO> UpdateCategoryAsync(CategoryDTO Entity);
        Task DeleteCategoryAsync(long CategoryId);
        Task<IEnumerable<CategoryDTO>> GetAllGeneralCategoriesAsync(long restaurantId);
        Task<CategoryDTO> ToggleActiveStatus(long CategoryId);
        Task<IEnumerable<CategoryDTO>> GetAllCategoriesDropDownByMenuIdAsync(long MenuId, long RestaurantId);
        Task<long> MaxPosition(long RestaurantId);
    }
}
