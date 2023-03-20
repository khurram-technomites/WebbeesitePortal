using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class CardSchemeRepo : Repository<CardScheme>, ICardSchemeRepo
    {
        public CardSchemeRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {
        }
    }
}
