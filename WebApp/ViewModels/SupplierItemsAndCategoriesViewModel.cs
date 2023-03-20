using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SupplierItemsAndCategoriesViewModel
    {
        public IEnumerable<SupplierItemViewModel> Items { get; set; }
        public IEnumerable<SupplierItemCategoryViewModel> Categories { get; set; }
    }
}
