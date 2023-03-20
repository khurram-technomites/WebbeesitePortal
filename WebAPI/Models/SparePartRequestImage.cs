using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class SparePartRequestImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(SparePartRequest))]
        public long SparePartRequestId { get; set; }
        public string Image { get; set; }
        public SparePartRequest SparePartRequest { get; set; }
    }
}
