using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Ticket : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string TicketNo {get;set;}
        [ForeignKey(nameof(Restaurant))]
        public long? RestaurantId { get; set; }
        [ForeignKey(nameof(Supplier))]
        public long? SupplierId { get; set; }
        [ForeignKey(nameof(user))]
        public string UserId { get; set; }
        public string Priority { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public Supplier Supplier { get; set; }
        public Restaurant Restaurant { get; set; }
        public AppUser user { get; set; }

    }
}
