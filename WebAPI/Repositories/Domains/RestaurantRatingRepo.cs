using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantRatingRepo : Repository<RestaurantRating>, IRestaurantRatingRepo
    {
        public RestaurantRatingRepo(FougitoContext context, ILoggerManager logger): base(context, logger)
        {

        }
    }
}
