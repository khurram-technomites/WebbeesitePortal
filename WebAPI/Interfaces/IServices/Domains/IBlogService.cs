using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IBlogService
    {
        Task<IEnumerable<Blogs>> GetAllBlogsAsync();
        Task<long> GetTotalCount();
        Task<IEnumerable<Blogs>> GetAllBlogsAsync(PagingParameters paging);
        Task<IEnumerable<Blogs>> GetBlogByIdAsync(long Id);
        Task<IEnumerable<Blogs>> GetBlogBySlugAsync(string Slug);
        Task<Blogs> AddBlogAsync(Blogs Entity);
        Task<Blogs> UpdateBlogAsync(Blogs Entity);
        Task<Blogs> ArchiveBlogAsync(long Id);
    }
}
