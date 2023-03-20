using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class SupplierCouponRepo : Repository<SupplierCoupon>, ISupplierCouponRepo
    {
        private new readonly FougitoContext _context;
        public SupplierCouponRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {
            _context = context;
        }

        public async Task<IEnumerable<SupplierCoupon>> GetCouponByCustomer(long SupplierId, long restaurantId)
        {
            List<long> customercoupons = await _context.RestaurantCoupons.Where(x => x.RestaurantId == restaurantId).Select(x => x.SupplierCouponId).ToListAsync();

            return await _context.SupplierCoupon.Include(x => x.RestaurantCoupons).Include(x => x.SupplierCouponRedemptions)
                .Where(x => (customercoupons.Contains(x.Id) || x.IsOpenToAll == true) && (x.SupplierId == SupplierId || x.SupplierId == null))
                .ToListAsync();
        }
    }
}
