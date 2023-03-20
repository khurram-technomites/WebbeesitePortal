using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class GarageExpertiseRepo:Repository<GarageExpertise>, IGarageExpertiseRepo
    {
        public GarageExpertiseRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }


        //public async Task<IEnumerable<GarageExpertise>> GetMenuByGarageId(long GarageId)
        //{
        //    var result2 = _context.GarageExpertise.Where(x => x.GarageId == GarageId).Select(x => x.).Distinct().ToList();
        //    var result = await _context.GarageExpertise.Where(x => !result2.Contains(x.Id) && x.ArchivedDate == null).ToListAsync();

        //    return result;
        //}
    }
}
