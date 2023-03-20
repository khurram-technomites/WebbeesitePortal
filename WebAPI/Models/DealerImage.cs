using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class DealerImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(SparePartsDealer))]
        public long SparePartDealerId { get; set; }
        public string Image { get; set; }
        public SparePartsDealer SparePartsDealer { get; set; }
    }
}
