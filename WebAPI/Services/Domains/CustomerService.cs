using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
	public class CustomerService : ICustomerService
	{
		private readonly ICustomerRepo _repo;
		private readonly IRestaurantCustomerRepo _restaruantCustomerRepo;
		private readonly ICustomerSessionRepo _customerSessionRepo;
		private readonly IRestaurantCustomerService _restaurantCustomerService;
		public CustomerService(ICustomerRepo repo, IRestaurantCustomerService restaurantCustomer, ICustomerSessionRepo customerSessionRepo
			, IRestaurantCustomerRepo restaruantCustomerRepo)
		{
			_repo = repo;
			_restaurantCustomerService = restaurantCustomer;
			_customerSessionRepo = customerSessionRepo;
			_restaruantCustomerRepo = restaruantCustomerRepo;
		}
		public async Task<Customer> AddCustomerAsync(Customer Model)
		{
			return await _repo.InsertAsync(Model);
		}

		public async Task<Customer> ArchiveCustomerAsync(long Id)
		{
			return await _repo.ArchiveAsync(Id);
		}

		public async Task<IEnumerable<Customer>> GetAllAsync()
		{
			return await _repo.GetAllAsync();
		}
		public async Task<long> GetAllCustomersCountAsync()
		{
			return await _repo.GetCount();
		}
		public async Task<IEnumerable<RestaurantCustomer>> GetCustomerByRestaurantIdAsync(long restaurantId)
		{
			var result = await _restaruantCustomerRepo.GetByIdAsync(x => x.RestaurantId == restaurantId, ChildObjects: "Customer");
			return result;
		}
		public async Task<IEnumerable<object>> GetAllCustomersDropDownByRestaurantIdAsync(long restaurantId)
		{

			var customer = await _repo.GetAllAsync();
			var restaurantCustomer = await _restaurantCustomerService.GetRestaurantCustomersAsync(restaurantId);

			var result = from rc in restaurantCustomer
						 join c in customer
						 on rc.CustomerId equals c.Id
						 select new
						 {

							 Id = c.Id,
							 Name = c.Name,

						 };
			return result;
		}
		public async Task<IEnumerable<object>> GetAllCustomersDropDownByAdminAsync()
		{

			var customer = await GetAllAsync();
			var result = from rc in customer
						 join c in customer
						 on rc.Id equals c.Id
						 select new
						 {
							 Id = c.Id,
							 Name = c.Name,
						 };
			return result;
		}


		public async Task<IEnumerable<Customer>> GetByIdAsync(long Id)
		{
			return await _repo.GetByIdAsync(x => x.Id == Id, ChildObjects: "CustomerAddresses");
		}
		public async Task<Customer> GetByContactAsync(string contact)
		{
			var list = await _repo.GetAllAsync(x => x.Contact == contact);

			return list.FirstOrDefault();
		}
		public async Task<IEnumerable<Customer>> GetByUserIdAsync(string UserId)
		{
			var list = await _repo.GetByIdAsync(x => x.UserId == UserId, ChildObjects: "User");
			return list;
		}

		public async Task<Customer> UpdateCustomerAsync(Customer Model)
		{
			return await _repo.UpdateAsync(Model);
		}

		public async Task<IEnumerable<CustomerSession>> GetCustomerSessionFirebaseTokens(long Id, bool? isPushNotificationAllowed)
		{
			return await _customerSessionRepo.GetByIdAsync(x => x.ID == Id);
		}
		public async Task<IEnumerable<CustomerSession>> GetCustomerSessions(long customerId, bool? isPushNotificationAllowed)
		{
			return await _customerSessionRepo.GetByIdAsync(x => x.CustomerID == customerId);
		}
	}
}
