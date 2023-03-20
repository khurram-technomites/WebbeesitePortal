using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SupplierCouponRedemptionService : ISupplierCouponRedemptionService
    {
        private readonly ISupplierCouponRedemptionRepo _repo;

        public SupplierCouponRedemptionService(ISupplierCouponRedemptionRepo repo)
        {
            _repo = repo;
        }

        public async Task<SupplierCouponRedemption> AddSupplierCouponRedemption(SupplierCouponRedemption Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<IEnumerable<SupplierCouponRedemption>> GetCouponRedemptionByRestaurant(long SupplierCouponId , string RestaurantId)
        {
            return await _repo.GetAllAsync(x => x.SupplierOrderId == SupplierCouponId && x.UserId == RestaurantId);
        }

        public async Task<IEnumerable<SupplierCouponRedemption>> GetCouponRedemption(long SupplierCouponId)
        {
            return await _repo.GetAllAsync(x => x.SupplierOrderId == SupplierCouponId);
        }
    }
}
