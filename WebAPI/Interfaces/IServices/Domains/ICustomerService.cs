using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<long> GetAllCustomersCountAsync();
        Task<IEnumerable<RestaurantCustomer>> GetCustomerByRestaurantIdAsync(long restaurantId);
        Task<IEnumerable<Customer>> GetByIdAsync(long Id);
        Task<Customer> GetByContactAsync(string contact);
        Task<IEnumerable<Customer>> GetByUserIdAsync(string UserId);
        Task<Customer> AddCustomerAsync(Customer Model);
        Task<IEnumerable<object>> GetAllCustomersDropDownByRestaurantIdAsync(long restaurantId);
        Task<IEnumerable<object>> GetAllCustomersDropDownByAdminAsync();
        Task<Customer> UpdateCustomerAsync(Customer Model);
        Task<Customer> ArchiveCustomerAsync(long Id);
    }
}
