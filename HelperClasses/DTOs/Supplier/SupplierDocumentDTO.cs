using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Supplier
{
    public class SupplierDocumentDTO
    {
        public long Id { get; set; }
        public long SupplierId { get; set; }
        public string Path { get; set; }
        public string DocumentType { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public SupplierDTO Supplier { get; set; }
    }
}
