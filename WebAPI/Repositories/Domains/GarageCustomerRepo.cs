using HelperClasses.Classes;
using HelperClasses.DTOs.Garage.Filter;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class GarageCustomerRepo : Repository<GarageCustomerInvoice>, IGarageCustomerRepo
    {
        private new readonly FougitoContext _context;

        public GarageCustomerRepo(FougitoContext context, ILoggerManager _logger) : base(context, _logger)
        {
            _context = context;
        }
        public async Task<IEnumerable<GarageCustomerInvoice>> GetByUserAndFilter(long GarageId, GarageWalletFilter Filter)
        {
            var result = _context.GarageCustomerInvoice.Where(x => x.GarageId == GarageId).AsQueryable();

            if (Filter.StartDate.HasValue && Filter.EndDate.HasValue)
                result = result.Where(x => x.CreationDate.Date >= Filter.StartDate.Value.Date && x.CreationDate.Date <= Filter.EndDate.Value.Date);
            return result.ToList();
        }
        public async Task<decimal> GetWallet(long GarageId)
        {
            var result = _context.GarageCustomerInvoice.Where(x => x.GarageId == GarageId && x.Status == Enum.GetName(typeof(GarageCustomerStatus), GarageCustomerStatus.UnPaid)).DefaultIfEmpty().Sum(x => Convert.ToDecimal(x.Total));
            return result;
        }
    }
}
