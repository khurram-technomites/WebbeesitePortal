using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class CarModelRepo : Repository<CarModel>, ICarModelRepo
    {
        public CarModelRepo(FougitoContext context, ILoggerManager loggerManager): base(context, loggerManager)
        {

        }
    }
}
