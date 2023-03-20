using HelperClasses.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class CustomerTransactionHistoryDTO
    {
        public long Id { get; set; }
        public string NameOnCard { get; set; }
        public string MaskCardNo { get; set; }
        public string TransactionStatus { get; set; }
        public decimal Amount { get; set; }
        public long OrderId { get; set; }
        public long CustomerId { get; set; }
        public string RestaurantName { get; set; }
        public DateTime CreationDate { get; set; }
        public OrderDTO Order { get; set; }
        public CustomerDTO Customer { get; set; }
    }
}
