using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SupplierItemCategory : GeneralSchema
    {
        public SupplierItemCategory()
        {
            SupplierItems = new HashSet<SupplierItem>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public ICollection<SupplierItem> SupplierItems { get; set; }
    }
}
