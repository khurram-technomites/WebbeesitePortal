using HelperClasses.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IBlogCategoryClient
    {
        Task<BlogCategoryDTO> AddBlogCategory(BlogCategoryDTO Model);
        Task<BlogCategoryDTO> UpdateBlogCategory(BlogCategoryDTO Model);
        Task<BlogCategoryDTO> ArchiveBlogCategory(long Id);
        Task<IEnumerable<BlogCategoryDTO>> GetAllBlogCategories();
        Task<IEnumerable<BlogCategoryDTO>> GetBlogCategoriesById(long Id);
        Task<IEnumerable<BlogCategoryDTO>> GetAllByGarageIdAsync(long GarageId);
        Task<IEnumerable<BlogCategoryDTO>> GetBlogCategoriesByModule(string Module);
    }
}
