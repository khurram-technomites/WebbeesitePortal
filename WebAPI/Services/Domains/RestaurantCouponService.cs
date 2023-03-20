using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Helpers;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class RestaurantCouponService : IRestaurantCouponService
    {
        private readonly IRestaurantCouponRepo _repo;
        private readonly ISupplierCouponRepo _customerRepo;
        private new readonly FougitoContext _context;
        public RestaurantCouponService(IRestaurantCouponRepo repo, ISupplierCouponRepo customerRepo, FougitoContext context)
        {
            _repo = repo;
            _customerRepo = customerRepo;
            _context = context;
        }
        public async Task<RestaurantCoupon> AddRestaurantCouponAsync(RestaurantCoupon Model)
        {
            return await _repo.InsertAsync(Model);
        }

        //public async Task<RestaurantCoupon> ArchiveRestaurantCouponAsync(long Id)
        //{
        //    return await _repo.ArchiveAsync(Id);
        //} UZAIF 

        public async Task<IEnumerable<RestaurantCoupon>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<RestaurantCoupon> UpdateRestaurantCouponAsync(RestaurantCoupon Model)
        {
            return await _repo.UpdateAsync(Model);
        }

        public async Task<IEnumerable<RestaurantCoupon>> GetCoupon(long CustomerID, long CouponsID)
        {
            return await _repo.GetAllAsync(x => x.RestaurantId == CustomerID && x.SupplierCouponId == CouponsID);
        }

        public async Task<IEnumerable<RestaurantCoupon>> GetCouponsByCouponID(long CouponsID)
        {
            return await _repo.GetAllAsync(x => x.SupplierCouponId == CouponsID, ChildObjects: "Restaurant");
        }

        public async Task<IEnumerable<SupplierCoupon>> GetRestaurantCoupons(long RestaurantId)
        {
            //IEnumerable<RestaurantCoupon> RestaurantCoupons = await _repo.GetAllAsync();
            //var customer = RestaurantCoupons.Select(x => x.Id);

            //var coupons = await _customerRepo.GetAllAsync();
            //return coupons.Where(x => (customer.Contains(x.Id) || x.IsOpenToAll == true)).ToList();
        
                List<long> customercoupons = await _context.RestaurantCoupons.Where(x => x.RestaurantId == RestaurantId).Select(x => x.SupplierCouponId).ToListAsync();

                List<SupplierCoupon> result = await _context.SupplierCoupon.Include(x => x.RestaurantCoupons).Include(x => x.SupplierCouponRedemptions)
                    .Where(x => (customercoupons.Contains(x.Id)  || x.IsOpenToAll == true))
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
        public async Task DeleteRestaurantCouponAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

    }
}
