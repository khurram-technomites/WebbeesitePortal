using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SupplierPackageViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string BillingPeriod { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public bool IsFree { get; set; }
        public int MonthCount { get; set; }
        public DateTime CreationDate { get; set; }
        public List<SupplierViewModel> Suppliers { get; set; }
    }
}
