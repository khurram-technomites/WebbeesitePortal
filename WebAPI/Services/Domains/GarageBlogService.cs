using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageBlogService: IGarageBlogService
    {
        private readonly IGarageBlogRepo _repo;
        public GarageBlogService(IGarageBlogRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GarageBlog>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageBlog>> GetGarageBlogByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<GarageBlog>> GetGarageBlogByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GaragedId, ChildObjects: "Garage");
        }
        public async Task<long> GetCountByGarageIdAsync(long GaragedId)
        {
            return await _repo.GetCount(x => x.GarageId == GaragedId);
        }
        public async Task<GarageBlog> AddGarageBlogAsync(GarageBlog Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<GarageBlog> UpdateGarageBlogAsync(GarageBlog Model)
        {
            return await _repo.UpdateAsync(Model);
        }
        public async Task<GarageBlog> ArchiveGarageBlogAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<GarageBlog>> GetGarageBlogBySlugAsync(string Slug)
        {
            return await _repo.GetByIdAsync(x => x.Slug == Slug);
        }

        public async Task<IEnumerable<GarageBlog>> GetGarageRelatedBlogByIdAsync(long BlogId, long CategoryId)
        {
            return await _repo.GetByIdAsync(x => x.Id != BlogId && x.BlogCategoryId == CategoryId);
        }
    }
}
