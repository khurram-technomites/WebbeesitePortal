using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantBannerSettingRepo : Repository<RestaurantBannerSetting>, IRestaurantBannerSettingRepo
    {
        public RestaurantBannerSettingRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
