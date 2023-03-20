using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Fatoorah.WebHook
{
    public class WebHookSupplierStatusDTO
    {
        public int SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string SupplierMobile { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierStatus { get; set; }
    }
}
