using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IBlogCategoryService
    {
        Task<BlogCategory> AddBlogCategory(BlogCategory Model);
        Task<BlogCategory> UpdateBlogCategory(BlogCategory Model);
        Task<BlogCategory> ArchiveBlogCategory(long Id);
        Task<IEnumerable<BlogCategory>> GetAllBlogCategories();
        Task<IEnumerable<BlogCategory>> GetBlogCategoriesById(long Id);
        Task<IEnumerable<BlogCategory>> GetBlogCategoriesByGarageId(long GarageId);

        Task<IEnumerable<BlogCategory>> GetBlogCategoriesByModule(string Module);
    }
}
