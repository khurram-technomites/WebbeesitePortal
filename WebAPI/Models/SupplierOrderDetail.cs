using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class SupplierOrderDetail : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(SupplierItem))]
        public long SupplierItemId { get; set; }
        [ForeignKey(nameof(SupplierOrder))]
        public long SupplierOrderId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice { get; set; }
        public string SupplierItemName { get; set; }
        public SupplierItem SupplierItem { get; set; }
        public SupplierOrder SupplierOrder { get; set; }
    }
}
