using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class SupplierCustomerService : ISupplierCustomerService
    {
        private readonly ISupplierCustomerRepo _repo;
        public SupplierCustomerService(ISupplierCustomerRepo repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<RestaurantCoupon>> GetSupplierCustomersAsync(long restaurantId)
        {
            IEnumerable<RestaurantCoupon> list = await _repo.GetAllAsync(x => x.RestaurantId == restaurantId);
            return list.ToList();
        }

        public async Task<RestaurantCoupon> AddSupplierCustomer(RestaurantCoupon Model)
        {
            return await _repo.InsertAsync(Model);
        }
    }
}
