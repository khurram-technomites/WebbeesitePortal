using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ISupplierCustomerService
    {
        Task<IEnumerable<RestaurantCoupon>> GetSupplierCustomersAsync(long restaurantId);
        Task<RestaurantCoupon> AddSupplierCustomer(RestaurantCoupon Model);
    }
}
