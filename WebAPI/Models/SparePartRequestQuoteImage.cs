using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartRequestQuoteImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(SparePartRequestQuote))]
        public long SparePartRequestQuoteId { get; set; }
        public string Image { get; set; }
        public SparePartRequestQuote SparePartRequestQuote { get; set; }
    }
}
