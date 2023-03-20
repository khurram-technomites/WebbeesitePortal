using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class BlogRepo : Repository<Blogs>, IBlogRepo
    {
        public BlogRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }

    }
}
