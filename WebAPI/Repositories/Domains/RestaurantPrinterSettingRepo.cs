using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantPrinterSettingRepo : Repository<RestaurantPrinterSetting>, IRestaurantPrinterSettingRepo
    {
        public RestaurantPrinterSettingRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
