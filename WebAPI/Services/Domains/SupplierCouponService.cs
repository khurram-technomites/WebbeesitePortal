using HelperClasses.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SupplierCouponService : ISupplierCouponService
    {
        private readonly ISupplierCouponRepo _repo;
        public SupplierCouponService(ISupplierCouponRepo repo)
        {
            _repo = repo;
        }
        public async Task<SupplierCoupon> AddCouponAsync(SupplierCoupon Model)
        {
            return await _repo.InsertAsync(Model);
        }

        public async Task<SupplierCoupon> ArchiveCouponAsync(long Id)
        {
            return await _repo.ArchiveAsync(Id);
        }
        public async Task<long> GetAllCouponsCountAsync(long SupplierId)
        {
            return await _repo.GetCount(x => x.SupplierId == SupplierId);
        }
        public async Task<IEnumerable<SupplierCoupon>> GetAllAsync(long supplierId = 0)
        {
            if (supplierId != 0)
            {
                return await _repo.GetAllAsync(x => x.SupplierId == supplierId);
            }

            else
            {
                return await _repo.GetAllAsync();
            }

        }

        //public async Task<IEnumerable<SupplierCoupon>> GetCustomerCoupon(long customerId)
        //{

        //    var customercoupons = _customerCouponRepo_.GetAll().Where(x => x.CustomerID == CustometId).Select(i => i.CouponsID).ToList();

        //    var coupons = _couponRepository.GetAll().Where(x => (customercoupons.Contains(x.ID) || x.IsOpenToAll == true) && x.IsDeleted == false && x.IsActive == true).ToList();
        //    return coupons;
        //}

        public async Task<IEnumerable<SupplierCoupon>> GetAllAdminAsync()
        {
            return await _repo.GetAllAsync(x => x.SupplierId == null);
        }

        public async Task<IEnumerable<SupplierCoupon>> GetByCodeAsync(string Code)
        {
            return await _repo.GetByIdAsync(x => x.CouponCode == Code && x.ArchivedDate == null && x.Status == Enum.GetName(typeof(Status), Status.Active), ChildObjects: "CustomerCoupons, CouponRedemptions, CouponCategories, CouponCategories.Category");
        }

        public async Task<IEnumerable<SupplierCoupon>> GetByCustomerAsync(long SupplierId, long CustomerId)
        {
            return await _repo.GetCouponByCustomer(SupplierId, CustomerId);
        }

        public async Task<IEnumerable<SupplierCoupon>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<SupplierCoupon> UpdateCouponAsync(SupplierCoupon Model)
        {
            return await _repo.UpdateAsync(Model);
        }
    }
}
