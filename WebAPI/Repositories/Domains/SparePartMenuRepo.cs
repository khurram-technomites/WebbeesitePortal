using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartMenuRepo : Repository<SparePartMenu>, ISparePartMenuRepo
    {
        public SparePartMenuRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }

        public async Task<IEnumerable<SparePartMenu>> GetMenuBySparePartDealerId(long SparePartDealerId)
        {
            var result2 = _context.SparePartMenusManagement.Where(x => x.SparePartDealerId == SparePartDealerId).Select(x => x.SparePartMenuId).Distinct().ToList();
            var result = await _context.SparePartMenus.Where(x => !result2.Contains(x.Id) && x.ArchivedDate == null).ToListAsync();

            return result;
        }
    }
}
