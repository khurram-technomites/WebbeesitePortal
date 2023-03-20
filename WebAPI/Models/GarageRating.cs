using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageRating : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }
        public string Status { get; set; }
        public DateTime? PublishedDatetime { get; set; }
        public AppUser User { get; set; }
        public Garage Garage { get; set; }
    }
}
