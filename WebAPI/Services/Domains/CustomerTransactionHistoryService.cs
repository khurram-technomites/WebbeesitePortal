using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Interfaces.IRepositories.Domains;
using WebAPI.Interfaces.IServices.Domains;
using WebAPI.Models;

namespace WebAPI.Services.Domains
{
    public class CustomerTransactionHistoryService : ICustomerTransactionHistoryService
    {
        private readonly ICustomerTransactionHistoryRepo _repo;

        public CustomerTransactionHistoryService(ICustomerTransactionHistoryRepo repo)
        {
            _repo = repo;
        }

        public async Task<CustomerTransactionHistory> AddTransactionAsync(CustomerTransactionHistory Model)
        {
            return await _repo.InsertAsync(Model);
        }
        public async Task<IEnumerable<CustomerTransactionHistory>> GetAllTransactionsAsync()
        {
            return await _repo.GetAllAsync( ChildObjects: "Order, Order.Restaurant, Customer");
        }
        public async Task<IEnumerable<CustomerTransactionHistory>> GetTransactionsByOrderIdAsync(long orderId)
        {
            return await _repo.GetByIdAsync(x => x.OrderId == orderId, ChildObjects: "Order , Customer");
        }
        public async Task<IEnumerable<CustomerTransactionHistory>> GetTransactionsByCustomerIdAsync(long customerId)
        {
            return await _repo.GetByIdAsync(x => x.CustomerId == customerId, ChildObjects: "Order , Customer");
        }
        public async Task<IEnumerable<CustomerTransactionHistory>> GetTransactionsByIdAsync(long Id)
        {
            return await _repo.GetByIdAsync( x => x.Id == Id, ChildObjects: "Order , Customer");
        }
    }
}
