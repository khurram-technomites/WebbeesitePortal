using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class UserRepo : Repository<AppUser>, IUserRepo
    {
        public UserRepo(FougitoContext _context, ILoggerManager _logger): base(_context, _logger)
        {

        }

        public int RecordCount()
        {
            return _context.Users.Count();
        }
    }
}
