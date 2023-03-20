using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartExpertiseManagementRepo : Repository<SparePartExpertiseManagement>, ISparePartExpertiseManagementRepo
    {
        public SparePartExpertiseManagementRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
