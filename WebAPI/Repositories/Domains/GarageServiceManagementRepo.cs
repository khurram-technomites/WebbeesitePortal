using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class GarageServiceManagementRepo:Repository<GarageServiceManagement>, IGarageServiceManagementRepo
    {
        public GarageServiceManagementRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
