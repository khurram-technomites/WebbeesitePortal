using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantTableReservationRepo:Repository<RestaurantTableReservation>, IRestaurantTableReservationRepo
    {
        public RestaurantTableReservationRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
