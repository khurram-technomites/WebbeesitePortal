using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISparePartBlogService
    {
        Task<IEnumerable<SparePartBlog>> GetAllAsync();
        Task<IEnumerable<SparePartBlog>> GetSparePartBlogByIdAsync(long Id);
        Task<IEnumerable<SparePartBlog>> GetSparePartRelatedBlogByIdAsync(long Id, long CategoryId);
        Task<IEnumerable<SparePartBlog>> GetSparePartBlogBySlugAsync(string Slug);
        Task<IEnumerable<SparePartBlog>> GetSparePartBlogBySparePartDealerIdAsync(long SparePartDealerId);
        Task<SparePartBlog> AddSparePartBlogAsync(SparePartBlog Model);
        Task<SparePartBlog> UpdateSparePartBlogAsync(SparePartBlog Model);
        Task<SparePartBlog> ArchiveSparePartBlogAsync(long Id);
    }
}
