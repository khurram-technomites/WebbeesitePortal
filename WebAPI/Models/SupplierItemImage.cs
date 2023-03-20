using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SupplierItemImage : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(SupplierItem))]
        public long SupplierItemId { get; set; }
        public string Path { get; set; }
        public SupplierItem SupplierItem { get; set; }
    }
}
