using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageMenuManagement : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }
        [ForeignKey(nameof(GarageMenu))]
        public long GarageMenuId { get; set; }
        public int Position { get; set; }
        public string Status { get; set; }
        public Garage Garage { get; set; }
        public GarageMenu GarageMenu { get; set; }
    }
}
