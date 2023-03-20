using Microsoft.EntityFrameworkCore;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class GarageSubscribersRepo:Repository<GarageSubscribers>, IGarageSubscribersRepo
    {
        public GarageSubscribersRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
