using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartBlogRepo : Repository<SparePartBlog>, ISparePartBlogRepo
    {
        public SparePartBlogRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
