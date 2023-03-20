using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CustomerCouponService : ICustomerCouponService
    {
        private readonly ICustomerCouponRepo _repo;
        private readonly ICouponRepo _customerRepo;
        public CustomerCouponService(ICustomerCouponRepo repo, ICouponRepo customerRepo)
        {
            _repo = repo;
            _customerRepo = customerRepo;
        }
        public async Task<CustomerCoupon> AddCustomerCouponAsync(CustomerCoupon Model)
        {
            return await _repo.InsertAsync(Model);
        }

        //public async Task<CustomerCoupon> ArchiveCustomerCouponAsync(long Id)
        //{
        //    return await _repo.ArchiveAsync(Id);
        //} UZAIF 

        public async Task<IEnumerable<CustomerCoupon>> GetByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync(x => x.Id == Id);
        }

        public async Task<CustomerCoupon> UpdateCustomerCouponAsync(CustomerCoupon Model)
        {
            return await _repo.UpdateAsync(Model);
        }

        public async Task<IEnumerable<CustomerCoupon>> GetCoupon(long CustomerID, long CouponsID)
        {
            return await _repo.GetAllAsync(x => x.CustomerId == CustomerID && x.CouponId == CouponsID);
        }
  
        public async Task<IEnumerable<CustomerCoupon>> GetCouponsByCouponID(long CouponsID)
        {
            return await _repo.GetAllAsync(x => x.CouponId == CouponsID , ChildObjects : "Customer");
        }

        public async Task<IEnumerable<Coupon>> GetCustomerCoupons(long CustometId)
        {
            IEnumerable<CustomerCoupon> customercoupons = await _repo.GetAllAsync();
            var customer = customercoupons.Select(x => x.Id);

            var coupons = await _customerRepo.GetAllAsync();
            return  coupons.Where(x => (customer.Contains(x.Id) || x.IsOpenToAll == true)).ToList();
   
        }
        public async Task DeleteCustomerCouponAsync(long Id)
        {
            await _repo.DeleteAsync(Id);
        }

    }
}
