using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageAppointmentManagement : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Section01 { get; set; }
        public string Section02 { get; set; }
        public string Section03 { get; set; }
        public string Section04 { get; set; }
        public Garage Garage { get; set; }
    }
}
