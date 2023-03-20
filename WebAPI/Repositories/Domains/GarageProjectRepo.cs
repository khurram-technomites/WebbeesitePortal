using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class GarageProjectRepo : Repository<GarageProject>, IGarageProjectRepo
    {
        public GarageProjectRepo(FougitoContext context, ILoggerManager logger) : base(context, logger)
        {
        }
    }
}
