using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class CustomerTransactionHistoryViewModel
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
        public RestaurantOrderViewModel Order { get; set; }
        public CustomerViewModel Customer { get; set; }
    }
}
