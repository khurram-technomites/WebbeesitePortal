using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SupplierPackage : GeneralSchema
    {
        public SupplierPackage()
        {
            Suppliers = new HashSet<Supplier>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string BillingPeriod { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public bool IsFree { get; set; }
        public int MonthCount { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }
    }
}
