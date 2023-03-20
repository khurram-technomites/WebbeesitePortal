using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartPartnersManagementRepo : Repository<SparePartPartnersManagement>, ISparePartPartnersManagementRepo
    {
        public SparePartPartnersManagementRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
