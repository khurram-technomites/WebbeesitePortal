using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SupplierDocumentViewModel
    {
        public long Id { get; set; }
        public long SupplierId { get; set; }
        public string Path { get; set; }
        public string DocumentType { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public SupplierViewModel Supplier { get; set; }
    }
}
