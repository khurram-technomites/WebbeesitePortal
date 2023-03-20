using HelperClasses.DTOs.Restaurant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class CustomerDTO
    {
        public CustomerDTO()
        {
            RestaurantCustomer = new List<RestaurantCustomerDTO>();
            CustomerAddresses = new List<CustomerAddressDTO>();
            TransactionHistories = new List<CustomerTransactionHistoryDTO>();
        }
        public long Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public string Logo { get; set; }
        public DateTime CreationDate { get; set; }
        public AppUserDTO User { get; set; }
        public List<RestaurantCustomerDTO> RestaurantCustomer { get; set; }
        public List<CustomerAddressDTO> CustomerAddresses { get; set; }
        public List<CustomerTransactionHistoryDTO> TransactionHistories { get; set; }
    }
}
