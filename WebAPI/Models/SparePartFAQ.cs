using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartFAQ : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(SparePart))]
        public long SparePartId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Position { get; set; }
        public SparePartsDealer SparePart { get; set; }
    }
}
