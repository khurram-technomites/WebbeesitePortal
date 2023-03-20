using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class RestaurantTransactionHistoryViewModel
    {
        public long Id { get; set; }
        public string NameOnCard { get; set; }
        public string MaskCardNo { get; set; }
        public string TransactionStatus { get; set; }
        public decimal Amount { get; set; }
        public long SupplierOrderId { get; set; }
        public long RestaurantId { get; set; }

        public SupplierOrderViewModel SupplierOrder { get; set; }
        public RestaurantViewModel Restaurant { get; set; }
    }
}
