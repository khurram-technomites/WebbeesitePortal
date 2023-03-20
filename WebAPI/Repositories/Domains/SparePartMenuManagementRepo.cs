using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartMenuManagementRepo : Repository<SparePartMenuManagement>, ISparePartMenuManagementRepo
    {
        public SparePartMenuManagementRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
