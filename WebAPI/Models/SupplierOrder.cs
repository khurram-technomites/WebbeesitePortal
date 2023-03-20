using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SupplierOrder : GeneralSchema
    {
        public SupplierOrder()
        {
            SupplierOrderDetails = new HashSet<SupplierOrderDetail>();
            SupplierCouponRedemptions = new HashSet<SupplierCouponRedemption>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string OrderNo { get; set; }
        [ForeignKey(nameof(Supplier))]
        public long SupplierId { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string DeliveryAddress { get; set; }
        public string Status { get; set; }
        public bool IsPaid { get; set; }
        public string Currency { get; set; }
        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }
        public string RestauantContact { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantEmail { get; set; }

        public string NoteToRider { get; set; }
        public string CouponCode { get; set; }
        public decimal CouponDiscount { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal TotalAmount { get; set; }
        public string InvoiceRef { get; set; }
        public bool PaymentCaptured { get; set; }
        public DateTime? OrderRequiredDate { get; set; }
        public TimeSpan? OrderRequiredTime { get; set; }
        public Restaurant Restaurant { get; set; }
        public Supplier Supplier { get; set; }
        public ICollection<SupplierOrderDetail> SupplierOrderDetails { get; set; }
        public ICollection<SupplierCouponRedemption> SupplierCouponRedemptions { get; set; }
        public RestaurantTransactionHistory RestaurantTransactionHistory { get; set; }
    }
}
