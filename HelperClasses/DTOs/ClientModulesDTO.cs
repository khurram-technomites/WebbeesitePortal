using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class ClientModulesDTO
    {
        public long Id { get; set; }
     
        public long ClientId { get; set; }

        public long ModuleId { get; set; }

        public long Quantity { get; set; }

        public decimal TotalPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Status { get; set; }

        public GarageDTO Garage { get; set; }

        public ModuleDTO Module { get; set; }
    }
    public class LayoutModuleDTO
    {

        public int Blog { get; set; }
        public int Service { get; set; }
        public int Appoinment { get; set; }
        public int Project { get; set; }
        public int Partner { get; set; }
        public int Teams { get; set; }
        public int Expertise { get; set; }
        public int Award { get; set; }
        public int Testimonial { get; set; }
        public int Feedback { get; set; }
    }
}
