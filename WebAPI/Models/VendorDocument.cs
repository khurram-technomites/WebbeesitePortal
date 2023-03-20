using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class VendorDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Type { get; set; }
        public string RefNumber { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public string Path { get; set; }
        [ForeignKey(nameof(Vendor))]
        public long VendorId { get; set; }
        public Vendor Vendor { get; set; }
    }
}
