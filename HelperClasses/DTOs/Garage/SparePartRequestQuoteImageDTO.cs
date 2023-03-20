using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperClasses.DTOs.Garage
{
    public class SparePartRequestQuoteImageDTO
    {
        public long Id { get; set; }
        public long SparePartRequestQuoteId { get; set; }
        public string Image { get; set; }
        public SparePartRequestQuoteDTO SparePartRequestQuote { get; set; }
    }
}
