using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SupplierItemCategoryRepo : Repository<SupplierItemCategory>, ISupplierItemCategoryRepo
    {
        public SupplierItemCategoryRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
