using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class RestaurantImages
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }
        public string Image { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
