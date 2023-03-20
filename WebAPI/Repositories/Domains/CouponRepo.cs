using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Models;

namespace WebAPI.Repositories.Domains
{
    public class CouponRepo : Repository<Coupon>, ICouponRepo
    {
        private new readonly FougitoContext _context;
        public CouponRepo(FougitoContext context, ILoggerManager loggerManager) : base(context, loggerManager)
        {
            _context = context;
        }

        public async Task<IEnumerable<Coupon>> GetCouponByCustomer(long RestaurantId, long CustomerId)
        {
            if(RestaurantId != 0)
            {
                List<long> customercoupons = await _context.CustomerCoupon.Where(x => x.CustomerId == CustomerId).Select(x => x.CouponId).ToListAsync();

                List<Coupon> result = await _context.Coupons.Include(x => x.CustomerCoupons).Include(x => x.CouponRedemptions)
                    .Where(x => (customercoupons.Contains(x.Id) || x.IsOpenToAll == true) && (x.RestaurantId == RestaurantId || x.RestaurantId == null))
                    .ToListAsync();

                foreach (var coupon in result.ToList())
                {
                    if ((DateTime.UtcNow.ToDubaiDateTime() - coupon.Expiry.Value).Days > 10)
                    {
                        result.Remove(coupon);
                    }
                }

                return result;
            }
            else
            {
                List<long> customercoupons = await _context.CustomerCoupon.Where(x => x.CustomerId == CustomerId).Select(x => x.CouponId).ToListAsync();

                List<Coupon> result = await _context.Coupons.Include(x => x.CustomerCoupons).Include(x => x.CouponRedemptions)
                    .Where(x => (customercoupons.Contains(x.Id) || x.IsOpenToAll == true))
                    .ToListAsync();

                foreach (var coupon in result.ToList())
                {
                    if ((DateTime.UtcNow.ToDubaiDateTime() - coupon.Expiry.Value).Days > 10)
                    {
                        result.Remove(coupon);
                    }
                }

                return result;
            }
        }
    }
}
