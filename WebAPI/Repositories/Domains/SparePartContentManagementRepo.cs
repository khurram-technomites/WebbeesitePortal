using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartContentManagementRepo : Repository<SparePartContentManagement>, ISparePartContentManagementRepo
    {
        public SparePartContentManagementRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
