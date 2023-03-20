using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SupplierItemCategoryViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public DateTime CreationDate { get; set; }
        public List<SupplierItemViewModel> SupplierItem { get; set; }
    }
}
