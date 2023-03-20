using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;


namespace WebAPI.Repositories.Domains
{
    public class RestaurantCustomerRepo : Repository<RestaurantCustomer>, IRestaurantCustomerRepo
    {
        public RestaurantCustomerRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
