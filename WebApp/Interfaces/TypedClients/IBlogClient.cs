using HelperClasses.DTOs;
using HelperClasses.DTOs.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface IBlogClient
    {
        Task<IEnumerable<BlogDTO>> GetBlogs();
        Task<BlogDTO> GetBlogByID(long Id);
        Task<BlogDTO> Create(BlogDTO model);
        Task<BlogDTO> Edit(BlogDTO model);

        Task<BlogDTO> Delete(long Id);
        Task<BlogDTO> ToggleActiveStatus(long Id);

    }
}
