using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Supplier
{
    public class SupplierOrderDetailDTO
    {
        public long Id { get; set; }
        public long SupplierItemId { get; set; }
        public long SupplierOrderId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string SupplierItemName { get; set; }
        public SupplierItemDTO SupplierItem { get; set; }
        public SupplierOrderDTO SupplierOrder { get; set; }
    }
}
