﻿using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SparePartCustomerAppointmentRepo : Repository<SparePartCustomerAppointment>, ISparePartCustomerAppointmentRepo
    {
        public SparePartCustomerAppointmentRepo(FougitoContext _context, ILoggerManager _logger) : base(_context, _logger)
        {

        }
    }
}
