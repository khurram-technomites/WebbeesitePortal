using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class TestTableRepo : Repository<TestTable>, ITestTableRepo
    {
        public TestTableRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
