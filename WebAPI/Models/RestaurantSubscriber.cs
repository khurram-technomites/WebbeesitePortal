using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class RestaurantSubscriber
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Email { get; set; }
        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
