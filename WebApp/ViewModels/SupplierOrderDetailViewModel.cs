using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SupplierOrderDetailViewModel
    {
        public long Id { get; set; }
        public long SupplierItemId { get; set; }
        public long SupplierOrderId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string SupplierItemName { get; set; }
        public SupplierItemViewModel SupplierItem { get; set; }
        public SupplierOrderViewModel SupplierOrder { get; set; }
    }
}
