using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Supplier
{
    public class SupplierItemDTO
    {
        public SupplierItemDTO()
        {
            SupplierItemImages = new List<SupplierItemImageDTO>();
            SupplierOrderDetails = new List<SupplierOrderDetailDTO>();
        }
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
        public long CategoryId { get; set; }
        public long SupplierId { get; set; }
        public string Thumbnail { get; set; }
        public DateTime CreationDate { get; set; }
        public SupplierDTO Supplier { get; set; }
        public SupplierItemCategoryDTO Category { get; set; }
        public List<SupplierItemImageDTO> SupplierItemImages { get; set; }
        public List<SupplierOrderDetailDTO> SupplierOrderDetails { get; set; }
    }
}
