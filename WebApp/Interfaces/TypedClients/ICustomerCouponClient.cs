using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ICustomerCouponClient
    {
        Task<CustomerCouponDTO> GetCustomerCoupon(long CustomerId , long CouponId);
        Task<IEnumerable<CustomerCouponDTO>> GetCustomerCouponsByCoupon(long CouponId);
        Task<IEnumerable<CustomerCouponDTO>> GetAllCustomerCoupons(long CustomerId);
        Task<CustomerCouponDTO> AddCustomerCouponAsync(CustomerCouponDTO Entity);
        Task<CustomerCouponDTO> UpdateCustomerCouponAsync(CustomerCouponDTO Entity);
        Task DeleteCustomerCouponAsync(long CustomerCouponId);
    }
}
