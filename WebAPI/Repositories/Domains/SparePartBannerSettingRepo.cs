using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartBannerSettingRepo : Repository<SparePartBannerSetting>, ISparePartBannerSettingRepo
    {
        public SparePartBannerSettingRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
