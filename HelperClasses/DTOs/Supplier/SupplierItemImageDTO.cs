using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Supplier
{
    public class SupplierItemImageDTO
    {
        public long Id { get; set; }
        public long SupplierItemId { get; set; }
        public string Path { get; set; }
        public SupplierItemDTO SupplierItem { get; set; }
    }
}
