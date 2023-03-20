using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SupplierCardViewModel
    {
        public long Id { get; set; }
        public string NameAsPerTradeLicense { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
    }
}
