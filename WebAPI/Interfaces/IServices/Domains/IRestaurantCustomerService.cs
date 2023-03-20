using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface IRestaurantCustomerService
    {
        Task<IEnumerable<RestaurantCustomer>> GetRestaurantCustomersAsync(long restaurantId);
        Task<RestaurantCustomer> GetCustomerByRestaurantIdAsync(long restaurantId, long Id);
        Task<IEnumerable<RestaurantCustomer>> GetRestaurantCustomersByContactAsync(long restaurantId, string contact = "");
        Task<IEnumerable<RestaurantCustomer>> GetRestaurantBranchCustomersAsync(long restaurantBranchId);
        Task<RestaurantCustomer> AddRestaurantCustomer(RestaurantCustomer Model);
    }
}
