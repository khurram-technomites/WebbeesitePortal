using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CustomerAddressService : ICustomerAddressService
    {
        private readonly ICustomerAddressRepo _repo;
        public CustomerAddressService(ICustomerAddressRepo repo)
        {
            _repo = repo;
        }

        public async Task<CustomerAddress> AddCustomerAddressAsync(CustomerAddress Entity)
        {
            return await _repo.InsertAsync(Entity);
        }

        public async Task<CustomerAddress> ArchiveCustomerAddressAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }

        public async Task DeleteCustomerAddressAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

        public async Task<IEnumerable<CustomerAddress>> GetAddressByCustomerAsync(long CustomerId)
        {
            return await _repo.GetByIdAsync(x => x.CustomerId == CustomerId);
        }

        public async Task<IEnumerable<CustomerAddress>> GetAllCustomerAddresssAsync(PagingParameters Pagination)
        {
            return await _repo.GetAllAsync(Pagination: Pagination, OrderExp: x => x.Id, ChildObjects: "User");
        }

        public async Task<IEnumerable<CustomerAddress>> GetCustomerAddressByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<CustomerAddress> UpdateCustomerAddressAsync(CustomerAddress Entity)
        {
            return await _repo.UpdateAsync(Entity);
        }
    }
}
