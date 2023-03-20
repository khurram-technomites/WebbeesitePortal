using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SupplierItem : GeneralSchema
    {
        public SupplierItem()
        {
            SupplierItemImages = new HashSet<SupplierItemImage>();
            SupplierOrderDetails = new HashSet<SupplierOrderDetail>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Packaging { get; set; }
        public DateTime? ExpiryDateTime { get; set; }
        public decimal RegularPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int Stock { get; set; }
        public int Threshold { get; set; }
        public bool IsManagedStock { get; set; }
        public string StockStatus { get; set; }
        [ForeignKey(nameof(Category))]
        public long CategoryId { get; set; }
        [ForeignKey(nameof(Supplier))]
        public long SupplierId { get; set; }
        public string Thumbnail { get; set; }
        public Supplier Supplier { get; set; }
        public SupplierItemCategory Category { get; set; }
        public ICollection<SupplierItemImage> SupplierItemImages { get; set; }
        public ICollection<SupplierOrderDetail> SupplierOrderDetails { get; set; }
    }
}
