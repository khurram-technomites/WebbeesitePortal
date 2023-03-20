using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SupplierOrderPlacementViewModel
    {
        public SupplierOrderPlacementViewModel()
        {
            SupplierOrderDetails = new();
        }
        public long SupplierId { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string NoteToRider { get; set; }
        public string StreetAddress { get; set; }
        public string Contact { get; set; }
        public decimal SupplierCouponDiscountAmount { get; set; }
        public Nullable<long> SupplierCouponId { get; set; }
        public DateTime? OrderRequiredDate { get; set; }
        public TimeSpan? OrderRequiredTime { get; set; }
        public List<SupplierOrderItemsViewModel> SupplierOrderDetails { get; set; }
    }

    public class SupplierOrderItemsViewModel
    {
        public long SupplierItemId { get; set; }
        public int Quantity { get; set; }
    }
}
