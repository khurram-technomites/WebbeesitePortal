using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;


namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICustomerAddressService
    {
        Task<IEnumerable<CustomerAddress>> GetAllCustomerAddresssAsync(PagingParameters Pagination);
        Task<IEnumerable<CustomerAddress>> GetCustomerAddressByIdAsync(long Id);
        Task<IEnumerable<CustomerAddress>> GetAddressByCustomerAsync(long CustomerId);
        Task<CustomerAddress> AddCustomerAddressAsync(CustomerAddress Entity);
        Task<CustomerAddress> UpdateCustomerAddressAsync(CustomerAddress Entity);
        Task<CustomerAddress> ArchiveCustomerAddressAsync(long Id);
        Task DeleteCustomerAddressAsync(long Id);
    }
}
