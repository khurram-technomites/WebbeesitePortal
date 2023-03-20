using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class RestaurantRating : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [ForeignKey(nameof(Restaurant))]
        public long RestaurantId { get; set; }
        [ForeignKey(nameof(Order))]
        public long? OrderId { get; set; }
        public string Status { get; set; }
        public bool ShowOnWebsite { get; set; }
        public DateTime? PublishedDatetime { get; set; }
        public AppUser User { get; set; }
        public Restaurant Restaurant { get; set; }
        public Order Order { get; set; }
    }
}
