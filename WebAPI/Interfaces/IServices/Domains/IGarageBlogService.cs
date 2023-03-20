using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IGarageBlogService
    {
        Task<IEnumerable<GarageBlog>> GetAllAsync();
        Task<IEnumerable<GarageBlog>> GetGarageBlogByIdAsync(long Id);
        Task<IEnumerable<GarageBlog>> GetGarageRelatedBlogByIdAsync(long BlogId, long CategoryId);
        Task<IEnumerable<GarageBlog>> GetGarageBlogBySlugAsync(string Slug);
        Task<IEnumerable<GarageBlog>> GetGarageBlogByGarageIdAsync(long GaragedId);
        Task<long> GetCountByGarageIdAsync(long GaragedId);
        Task<GarageBlog> AddGarageBlogAsync(GarageBlog Model);
        Task<GarageBlog> UpdateGarageBlogAsync(GarageBlog Model);
        Task<GarageBlog> ArchiveGarageBlogAsync(long Id);
    }
}
