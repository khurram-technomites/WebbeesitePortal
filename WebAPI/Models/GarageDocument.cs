using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Type { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public string Path { get; set; }
        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }
        public Garage Garage { get; set; }
    }
}
