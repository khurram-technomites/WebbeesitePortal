﻿using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class CarMakeRepo : Repository<CarMake>, ICarMakeRepo
    {
        public CarMakeRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
