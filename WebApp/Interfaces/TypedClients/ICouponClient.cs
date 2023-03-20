using HelperClasses.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ICouponClient
    {
        Task<IEnumerable<CouponDTO>> GetAllCouponsAsync(long RestaurantId = 0);
        Task<IEnumerable<CouponDTO>> GetAllAdminCouponsAsync();
        Task<CouponDTO> GetCouponByIdAsync(long CouponId);
        Task<CouponDTO> AddCouponAsync(CouponDTO Entity);
        Task<CouponDTO> UpdateCouponAsync(CouponDTO Entity);
        Task<CouponDTO> DeleteCouponAsync(long CouponId);
        Task<CouponDTO> ToggleActiveStatus(long CouponId);
    }
}
