using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class GarageRatingRepo : Repository<GarageRating> , IGarageRatingRepo
    {
        public GarageRatingRepo(FougitoContext context, ILoggerManager logger): base(context, logger)
        {

        }
    }
}
