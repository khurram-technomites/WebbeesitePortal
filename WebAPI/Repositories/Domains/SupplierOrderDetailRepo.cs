using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SupplierOrderDetailRepo:Repository<SupplierOrderDetail>, ISupplierOrderDetailRepo
    {
        public SupplierOrderDetailRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
