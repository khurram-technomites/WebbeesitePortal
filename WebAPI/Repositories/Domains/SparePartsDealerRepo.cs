using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains

{
    public class SparePartsDealerRepo : Repository<SparePartsDealer>, ISparePartDealRepo
    {
        public SparePartsDealerRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
