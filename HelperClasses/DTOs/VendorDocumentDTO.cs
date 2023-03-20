using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class VendorDocumentDTO
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public string FormattedDate { get { return ExpiryDateTime.ToShortDateString(); } set { } }
        public string Path { get; set; }
        public string RefNumber { get; set; }
        public long VendorId { get; set; }
    }
}
