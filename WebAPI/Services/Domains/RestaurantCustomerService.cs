using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
	public class RestaurantCustomerService : IRestaurantCustomerService
	{
		private readonly IRestaurantCustomerRepo _repo;
		public RestaurantCustomerService(IRestaurantCustomerRepo repo)
		{
			_repo = repo;
		}

		public async Task<IEnumerable<RestaurantCustomer>> GetRestaurantCustomersAsync(long restaurantId)
		{
			IEnumerable<RestaurantCustomer> list = await _repo.GetAllAsync(x => x.RestaurantId == restaurantId, ChildObjects: "Customer, Customer.CustomerAddresses");

			return list.ToList();
		}
		public async Task<RestaurantCustomer> GetCustomerByRestaurantIdAsync(long restaurantId, long Id)
		{
			IEnumerable<RestaurantCustomer> list  = await _repo.GetByIdAsync(x => x.RestaurantId == restaurantId && x.CustomerId == Id, ChildObjects: "");
			
			return list.FirstOrDefault();
		}

		public async Task<IEnumerable<RestaurantCustomer>> GetRestaurantCustomersByContactAsync(long restaurantId, string contact = "")
		{
			IEnumerable<RestaurantCustomer> list = new List<RestaurantCustomer>();

			if (!string.IsNullOrWhiteSpace(contact))
				list = await _repo.GetAllAsync(x => x.RestaurantId == restaurantId && (x.Customer != null && x.Customer.Contact.Contains(contact)), ChildObjects: "Customer, Customer.CustomerAddresses");
			else
			{
				list = await _repo.GetAllAsync(x => x.RestaurantId == restaurantId, ChildObjects: "Customer, Customer.CustomerAddresses");
				list = list.Take(20);
			}

			return list.ToList();
		}

		public async Task<IEnumerable<RestaurantCustomer>> GetRestaurantBranchCustomersAsync(long restaurantBranchId)
		{
			IEnumerable<RestaurantCustomer> list = await _repo.GetAllAsync(x => x.RestaurantBranchId == restaurantBranchId, ChildObjects: "Customer");
			return list.ToList();
		}

		public async Task<RestaurantCustomer> AddRestaurantCustomer(RestaurantCustomer Model)
		{
			return await _repo.InsertAsync(Model);
		}

	}
}
