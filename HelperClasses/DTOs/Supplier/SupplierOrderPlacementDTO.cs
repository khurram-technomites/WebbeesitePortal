using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Supplier
{
    public class SupplierOrderPlacementDTO
    {
        public long SupplierId { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string NoteToRider { get; set; }
        public string StreetAddress { get; set; }
        public string Contact { get; set; }
        public Nullable<long> SupplierCouponId { get; set; }
        public decimal SupplierCouponDiscountAmount { get; set; }
        public DateTime? OrderRequiredDate { get; set; }
        public TimeSpan? OrderRequiredTime { get; set; }
        public List<SupplierOrderItemsDTO> SupplierOrderDetails { get; set; } = new();
    }

    public class SupplierOrderItemsDTO
    {
        public long SupplierItemId { get; set; }
        public int Quantity { get; set; }
    }
}
