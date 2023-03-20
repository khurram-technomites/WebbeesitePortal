using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class BlogCategoryService : IBlogCategoryService
    {
        private readonly IBlogsCategoryRepo _repo;

        public BlogCategoryService(IBlogsCategoryRepo repo)
        {
            _repo = repo;
        }

        public async Task<BlogCategory> AddBlogCategory(BlogCategory Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<BlogCategory> ArchiveBlogCategory(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<BlogCategory>> GetAllBlogCategories()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<BlogCategory>> GetBlogCategoriesById(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }
        public async Task<IEnumerable<BlogCategory>> GetBlogCategoriesByGarageId(long Id)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == Id);
        }

        public async Task<IEnumerable<BlogCategory>> GetBlogCategoriesByModule(string Module)
        {
            return await _repo.GetByIdAsync(x => x.Module == Module);
        }

        public async Task<BlogCategory> UpdateBlogCategory(BlogCategory Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
