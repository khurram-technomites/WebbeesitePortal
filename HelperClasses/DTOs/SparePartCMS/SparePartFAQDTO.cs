using HelperClasses.DTOs.SparePartsDealer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.SparePartCMS
{
    public class SparePartFAQDTO
    {
        public long Id { get; set; }
        public long SparePartId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int Position { get; set; }
        public SparePartsDealerDTO SparePart { get; set; }
    }
}
