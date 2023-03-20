using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartTestimonialRepo : Repository<SparePartTestimonial>, ISparePartTestimonialRepo
    {
        public SparePartTestimonialRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {

        }
    }
}
