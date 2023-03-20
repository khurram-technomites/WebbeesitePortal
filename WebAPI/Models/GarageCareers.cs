using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageCareers : GeneralSchema
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }
        public string FulName { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
        public string Education { get; set; }
        public string Position { get; set; }
        public string Experience { get; set; }
        public string CVPath { get; set; }
        public Garage Garage { get; set; }
    }
}
