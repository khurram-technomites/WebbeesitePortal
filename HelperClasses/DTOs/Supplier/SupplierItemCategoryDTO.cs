using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Supplier
{
    public class SupplierItemCategoryDTO
    {

        public SupplierItemCategoryDTO()
        {
            SupplierItems = new List<SupplierItemDTO>();
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }

        public DateTime CreationDate { get; set; }
        public List<SupplierItemDTO> SupplierItems { get; set; }

    }
}
