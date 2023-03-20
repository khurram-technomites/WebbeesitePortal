using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class AggregatorRepo:Repository<Aggregator>, IAggregatorRepo
    {
        public AggregatorRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {
        }
    }
}
