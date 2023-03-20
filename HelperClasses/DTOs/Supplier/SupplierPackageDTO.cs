using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Supplier
{
    public class SupplierPackageDTO
    {
        public SupplierPackageDTO()
        {
            Suppliers = new List<SupplierDTO>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public string BillingPeriod { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public bool IsFree { get; set; }
        public int MonthCount { get; set; }

        public DateTime CreationDate { get; set; }
        public List<SupplierDTO> Suppliers { get; set; }
    }
}
