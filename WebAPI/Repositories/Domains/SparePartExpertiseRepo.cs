using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartExpertiseRepo : Repository<SparePartExpertise>, ISparePartExpertiseRepo
    {
        public SparePartExpertiseRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
