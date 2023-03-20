using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartCustomerFeedbackRepo : Repository<SparePartCustomerFeedback>, ISparePartCustomerFeedbackRepo
    {
        public SparePartCustomerFeedbackRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
