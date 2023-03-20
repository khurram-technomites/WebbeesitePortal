using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class GarageCustomerFeedbackRepo:Repository<GarageCustomerFeedback>, IGarageCustomerFeedbackRepo
    {
        public GarageCustomerFeedbackRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {
            
        }
    }
}
