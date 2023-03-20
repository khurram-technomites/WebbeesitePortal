using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Interfaces.IServices.Domains
{
    public interface ICustomerTransactionHistoryService
    {
        Task<CustomerTransactionHistory> AddTransactionAsync(CustomerTransactionHistory Model);
        Task<IEnumerable<CustomerTransactionHistory>> GetAllTransactionsAsync();
        Task<IEnumerable<CustomerTransactionHistory>> GetTransactionsByOrderIdAsync(long orderId);
        Task<IEnumerable<CustomerTransactionHistory>> GetTransactionsByCustomerIdAsync(long customerId);
        Task<IEnumerable<CustomerTransactionHistory>> GetTransactionsByIdAsync(long Id);
    }
}
