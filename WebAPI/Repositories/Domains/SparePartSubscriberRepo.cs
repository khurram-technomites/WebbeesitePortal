using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartSubscriberRepo : Repository<SparePartSubscriber>, ISparePartSubscriberRepo
    {
        public SparePartSubscriberRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
