using HelperClasses.DTOs.Garage.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices
{
    public interface IGarageCustomerService
    {
        Task<IEnumerable<GarageCustomerInvoice>> GetCustomerInvoiceByIdAsync(long Id);
        Task<IEnumerable<GarageCustomerInvoice>> GetAllWalletForFilterAsync(GarageWalletFilter Filter, long GarageID);
        Task<IEnumerable<GarageCustomerInvoice>> GetCustomerInvoiceByGarageIdAsync(long GarageId);
        Task<decimal> getWallet(long GarageId);
        Task<IEnumerable<GarageCustomerInvoice>> GetAllCustomerInvoicesAsync();
        Task<GarageCustomerInvoice> AddCustomerInvoiceAsync(GarageCustomerInvoice Entity);
        Task<GarageCustomerInvoice> UpdateCustomerInvoiceAsync(GarageCustomerInvoice Entity);
        Task<GarageCustomerInvoice> ArchiveCustomerInvoiceAsync(long Id);
    }
}
