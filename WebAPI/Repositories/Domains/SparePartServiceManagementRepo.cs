using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartServiceManagementRepo : Repository<SparePartServiceManagement>, ISparePartServiceManagementRepo
    {
        public SparePartServiceManagementRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
