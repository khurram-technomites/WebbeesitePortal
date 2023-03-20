using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.ViewModels
{
    public class SupplierItemImagesViewModel
    {
        public long Id { get; set; }
        public long SupplierItemId { get; set; }
        public string Path { get; set; }
        public SupplierItemViewModel SupplierItem { get; set; }
    }
}
