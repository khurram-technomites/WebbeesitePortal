using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SupplierDocument : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Supplier))]
        public long SupplierId { get; set; }
        public string Path { get; set; }
        public string DocumentType { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public Supplier Supplier { get; set; }
    }
}
