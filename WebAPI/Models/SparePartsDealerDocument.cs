using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartsDealerDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Type { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public string Path { get; set; }
        [ForeignKey(nameof(SparePartsDealer))]
        public long SparePartsDealerId { get; set; }
        public SparePartsDealer SparePartsDealer { get; set; }
    }
}
