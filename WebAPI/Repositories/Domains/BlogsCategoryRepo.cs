using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class BlogsCategoryRepo : Repository<BlogCategory>, IBlogsCategoryRepo
    {
        public BlogsCategoryRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {
        }
    }
}
