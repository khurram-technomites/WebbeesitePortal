using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageAward : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [MaxLength(50, ErrorMessage = "Name length must be less than 50 characters")]
        public string Name { get; set; }
        [MaxLength(500, ErrorMessage = "Description length must be less than 500 characters")]
        public string Description { get; set; }
        public string ImagePath { get; set; }

        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }
        public Garage Garage { get; set; }
    }
}
