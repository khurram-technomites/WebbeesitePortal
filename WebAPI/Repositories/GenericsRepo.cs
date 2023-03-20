using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class GenericsRepo<T> : Repository<T>, IGenericsRepo<T> where T : class
    {
        public GenericsRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {
        }
    }
}
