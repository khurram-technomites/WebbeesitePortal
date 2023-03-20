using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs
{
    public class ModulePurchaseDetailsDTO
    {
       
        public long Id { get; set; }
   
        public long ClientModulePurchaseID { get; set; }
      
        public long ModuleID { get; set; }
        public long Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public ClientModulePurchasesDTO ClientModulePurchases { get; set; }
        public ModuleDTO Module { get; set; }
    }
}
