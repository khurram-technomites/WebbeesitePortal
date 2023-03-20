using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class GarageExpertiseManagement : GeneralSchema
    {
        public GarageExpertiseManagement()
        {
            GarageExpertise = new HashSet<GarageExpertise>();
        }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [ForeignKey(nameof(Garage))]
        public long GarageId { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public Garage Garage { get; set; }
        public ICollection<GarageExpertise> GarageExpertise { get; set; }
    }
}
