using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageSchedule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }
        [MaxLength(10, ErrorMessage = "Day length must be less than 10 characters")]
        public string Day { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public bool IsBreak { get; set; }
        public TimeSpan? BreakTimeStart { get; set; }
        public TimeSpan? BreakTimeEnd { get; set; }
        public Garage Garage { get; set; }
    }
}
