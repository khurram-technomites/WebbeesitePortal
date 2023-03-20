using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Supplier
{
    public class SupplierCardDTO
    {
        public long Id { get; set; }
        public string NameAsPerTradeLicense { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
    }
}
