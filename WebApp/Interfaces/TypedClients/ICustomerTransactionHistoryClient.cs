using HelperClasses.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Interfaces.TypedClients
{
    public interface ICustomerTransactionHistoryClient
    {
        Task<IEnumerable<CustomerTransactionHistoryDTO>> GetAllTransactionsAsync();
        Task<IEnumerable<CustomerTransactionHistoryDTO>> GetTransactionsByOrderIdAsync(long Id);
        Task<IEnumerable<CustomerTransactionHistoryDTO>> GetTransactionsByCustomerIdAsync(long Id);
        Task<IEnumerable<CustomerTransactionHistoryDTO>> GetTransactionsByIdAsync(long Id);
    }
}
