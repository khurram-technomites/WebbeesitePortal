using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class GaragePartnersManagementRepo:Repository<GaragePartnersManagement>, IGaragePartnersManagementRepo
    {
        public GaragePartnersManagementRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
