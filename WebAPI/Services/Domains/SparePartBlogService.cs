using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SparePartBlogService : ISparePartBlogService
    {
        private readonly ISparePartBlogRepo _repo;
        public SparePartBlogService(ISparePartBlogRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SparePartBlog>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<SparePartBlog>> GetSparePartBlogByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<SparePartBlog>> GetSparePartBlogBySparePartDealerIdAsync(long sparePartDealerId)
        {
            return await _repo.GetByIdAsync(x => x.SparePartDealerId == sparePartDealerId, ChildObjects: "SparePartDealer");
        }

        public async Task<SparePartBlog> AddSparePartBlogAsync(SparePartBlog Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SparePartBlog> UpdateSparePartBlogAsync(SparePartBlog Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<SparePartBlog> ArchiveSparePartBlogAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<SparePartBlog>> GetSparePartBlogBySlugAsync(string Slug)
        {
            return await _repo.GetByIdAsync(x => x.Slug == Slug);
        }

        public async Task<IEnumerable<SparePartBlog>> GetSparePartRelatedBlogByIdAsync(long Id, long CategoryId)
        {
            return await _repo.GetByIdAsync(x => x.Id != Id && x.BlogCategoryId == CategoryId);
        }
    }
}
