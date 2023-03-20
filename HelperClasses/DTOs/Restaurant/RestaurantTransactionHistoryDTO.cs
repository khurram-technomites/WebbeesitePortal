using HelperClasses.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Restaurant
{
    public class RestaurantTransactionHistoryDTO
    {
        public long Id { get; set; }
        public string NameOnCard { get; set; }
        public string MaskCardNo { get; set; }
        public string TransactionStatus { get; set; }
        public decimal Amount { get; set; }
        public long SupplierOrderId { get; set; }
        public long RestaurantId { get; set; }

        public SupplierOrderDTO SupplierOrder { get; set; }
        public RestaurantDTO Restaurant { get; set; }
    }
}
