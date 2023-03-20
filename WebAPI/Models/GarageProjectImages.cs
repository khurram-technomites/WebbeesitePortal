using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageProjectImages
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(GarageProject))]
        public long GarageProjectId { get; set; }
        public string ImagePath { get; set; }
        public GarageProject GarageProject { get; set; }
    }
}
