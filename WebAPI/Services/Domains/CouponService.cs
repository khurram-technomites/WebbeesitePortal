using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepo _repo;
        public CouponService(ICouponRepo repo)
        {
            _repo = repo;
        }
        public async Task<Coupon> AddCouponAsync(Coupon Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<Coupon> ArchiveCouponAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<long> GetAllCouponsCountAsync(long RestaurantId)
        {
            return await _repo.GetCount(x => x.RestaurantId == RestaurantId);
        }
        public async Task<IEnumerable<Coupon>> GetAllAsync(long restaurantId = 0)
        {
            if (restaurantId != 0)
            {
                return await _repo.GetAllAsync(x => x.RestaurantId == restaurantId);
            }

            else
            {
                return await _repo.GetAllAsync();
            }

        }

        public async Task<IEnumerable<Coupon>> GetAllAdminAsync()
        {
                return await _repo.GetAllAsync(x => x.RestaurantId == null);
        }

        public async Task<IEnumerable<Coupon>> GetByCodeAsync(string Code)
        {
            return await _repo.GetByIdAsync(x => x.CouponCode == Code && x.ArchivedDate == null && x.Status == Enum.GetName(typeof(Status), Status.Active), ChildObjects: "CustomerCoupons, CouponRedemptions, CouponCategories, CouponCategories.Category");
        }

        public async Task<IEnumerable<Coupon>> GetByCodeByCustomerAsync(string Code)
        {
            return await _repo.GetByIdAsync(x => x.CouponCode == Code && x.ArchivedDate == null && x.Status == Enum.GetName(typeof(Status), Status.Active), ChildObjects: "CustomerCoupons, CouponRedemptions, CouponCategories, CouponCategories.Category");
        }

        public async Task<IEnumerable<Coupon>> GetByCustomerAsync(long RestaurantId, long CustomerId)
        {
            return await _repo.GetCouponByCustomer(RestaurantId, CustomerId);
        }

        public async Task<IEnumerable<Coupon>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<Coupon> UpdateCouponAsync(Coupon Model)
        {
            return await _repo.UpdateAsync(Model);
        }


    }
}
