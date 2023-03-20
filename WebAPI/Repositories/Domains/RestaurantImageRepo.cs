using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class RestaurantImageRepo : Repository<RestaurantImages>, IRestaurantImageRepo
    {
        private new readonly FougitoContext _context;
        private readonly IMapper _mapper;
        public RestaurantImageRepo(FougitoContext context, ILoggerManager logger, IMapper mapper) : base(context, logger)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
