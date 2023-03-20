using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartRequestQuoteImageRepo : Repository<SparePartRequestQuoteImage>, ISparePartRequestQuoteImageRepo
    {
        public SparePartRequestQuoteImageRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
