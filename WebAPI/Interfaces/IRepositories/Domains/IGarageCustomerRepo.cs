using HelperClasses.DTOs.Garage.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IRepositories.Domains
{
    public interface IGarageCustomerRepo : IRepository<GarageCustomerInvoice>
    {
        Task<IEnumerable<GarageCustomerInvoice>> GetByUserAndFilter(long GarageId, GarageWalletFilter Filter);
        Task<decimal> GetWallet(long GarageId);
    }
}
