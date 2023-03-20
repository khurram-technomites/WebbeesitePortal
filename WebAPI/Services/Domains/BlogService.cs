using HelperClasses.Classes;
using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepo _repo;
        public BlogService(IBlogRepo repo)
        {
            _repo = repo;
        }

        public async Task<Blogs> AddBlogAsync(Blogs Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<Blogs> ArchiveBlogAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task<IEnumerable<Blogs>> GetAllBlogsAsync()
        {
            return await _repo.GetAllAsync(OrderExp: x => x.Id);
        }

        public async Task<IEnumerable<Blogs>> GetAllBlogsAsync(PagingParameters paging)
        {
            return await _repo.GetAllAsync(x => x.Status == Enum.GetName(typeof(Status), Status.Active), Pagination: paging, OrderExp: x => x.Id);
        }

        public async Task<IEnumerable<Blogs>> GetBlogByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<IEnumerable<Blogs>> GetBlogBySlugAsync(string Slug)
        {
            return await _repo.GetByIdAsync(x => x.Slug == Slug);
        }

        public async Task<long> GetTotalCount()
        {
            return await _repo.GetCount();
        }

        public async Task<Blogs> UpdateBlogAsync(Blogs Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
