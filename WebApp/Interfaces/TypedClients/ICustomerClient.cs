using HelperClasses.DTOs;
using HelperClasses.DTOs.Restaurant;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ICustomerClient
    {
        Task<IEnumerable<RestaurantCustomerDTO>> GetAllCustomersByRestaurantAsync(long RestaurantId);
        Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync();
        Task<IEnumerable<RestaurantDTO>> GetAllSupplierCustomersAsync();
        Task<IEnumerable<CustomerDTO>> GetCustomers();
        Task<IEnumerable<object>> GetAllCustomerDropDownByAdminAsync();
        Task<IEnumerable<object>> GetAllCustomerDropDownByRestaurant(long RestaurantId);
        Task<CustomerDTO> GetCustomerByIdAsync(long CustomerId);
        //Task<CustomerDTO> AddCustomerAsync(CustomerDTO Entity);

        Task<CustomerDTO> UpdateCustomerAsync(CustomerDTO Entity);
        Task DeleteCustomerAsync(long CustomerId);
        Task<CustomerDTO> ToggleActiveStatus(long CustomerId);
    }
}
