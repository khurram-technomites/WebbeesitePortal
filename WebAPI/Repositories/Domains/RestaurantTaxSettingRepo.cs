using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantTaxSettingRepo : Repository<RestaurantTaxSetting>, IRestaurantTaxSettingRepo
    {
        public RestaurantTaxSettingRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
