using HelperClasses.DTOs.Garage.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class GarageCustomerService : IGarageCustomerService
    {
        private readonly IGarageCustomerRepo _repo;

        public GarageCustomerService(IGarageCustomerRepo repo)
        {
            _repo = repo;
        }
        public async Task<GarageCustomerInvoice> AddCustomerInvoiceAsync(GarageCustomerInvoice Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<GarageCustomerInvoice> ArchiveCustomerInvoiceAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<IEnumerable<GarageCustomerInvoice>> GetAllCustomerInvoicesAsync()
        {
            return await _repo.GetAllAsync();
        }
        public async Task<IEnumerable<GarageCustomerInvoice>> GetCustomerInvoiceByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "Garage", OrderExp: x => x.CreationDate, IsOrderByDescending: true);
        }
        public async Task<IEnumerable<GarageCustomerInvoice>> GetCustomerInvoiceByGarageIdAsync(long GarageId)
        {
            return await _repo.GetByIdAsync(x => x.GarageId == GarageId, ChildObjects: "Garage", OrderExp: x => x.CreationDate, IsOrderByDescending: true);
        }
        public async Task<GarageCustomerInvoice> UpdateCustomerInvoiceAsync(GarageCustomerInvoice Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
        public async Task<IEnumerable<GarageCustomerInvoice>> GetAllWalletForFilterAsync(GarageWalletFilter Filter, long GarageId)
        {
            return await _repo.GetByUserAndFilter(GarageId, Filter);
        }
        public async Task<decimal> getWallet(long GarageId)
        {
            return await _repo.GetWallet(GarageId);
        }

    }
}
