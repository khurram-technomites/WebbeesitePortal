using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class RestaurantDocument
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Type { get; set; }
        public DateTime ExpiryDateTime { get; set; }
        public string Path { get; set; }
        [ForeignKey(nameof(Restaurant))]
        public long ResturantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
