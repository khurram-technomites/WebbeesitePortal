using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartCareerRepo : Repository<SparePartCareer>, ISparePartCareerRepo
    {
        public SparePartCareerRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {

        }
    }
}
