using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;
namespace WebAPI.Repositories.Domains
{
    public class VendorDocumentRepo : Repository<VendorDocument>, IVendorDocumentRepo
    {
        public VendorDocumentRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
